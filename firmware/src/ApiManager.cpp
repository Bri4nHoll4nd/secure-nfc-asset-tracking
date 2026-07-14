#include "ApiManager.h"
#include "AppConfig.h"
#include "WiFiManager.h"

#include <Arduino.h>
#include <ArduinoJson.h>
#include <HTTPClient.h>

ApiManager::ApiManager(WiFiManager& manager) : wifiManager(manager) {
}

bool ApiManager::sendTagUid(std::string tagUid) {
    if (!wifiManager.isConnected()) {
        Serial.println("Cannot send: WiFi disconnected");
        return false;
    }
    
    HTTPClient http;

    String url =
        String(AppConfig::API_BASE_URL) +
        "/1.0/V1Tags/ScanTagUid";

    http.begin(url);
    http.addHeader("Content-Type", "application/json");

    JsonDocument document;
    document["tagUid"] = tagUid;

    String requestBody;
    serializeJson(document, requestBody);

    int statusCode = http.POST(requestBody);

    Serial.print("POST status: ");
    Serial.println(statusCode);

    if (statusCode > 0) {
        
        String response = http.getString();

        Serial.print("Response: ");
        Serial.println(response);
    } else {
        Serial.print("Request failed: ");
        Serial.println(http.errorToString(statusCode));
    }

    http.end();

    return statusCode >= 200 && statusCode < 300;
}

bool ApiManager::sendTagData(TagReadData tagData) {
    if (!wifiManager.isConnected()) {
        Serial.println("Cannot send: WiFi disconnected");
        return false;
    }
    
    HTTPClient http;

    String url =
        String(AppConfig::API_BASE_URL) +
        "/1.0/V1Tags/ScanTag";

    http.begin(url);
    http.addHeader("Content-Type", "application/json");

    JsonDocument document;
    document["tagUid"] = tagData.tagUid;
    document["tagId"] = tagData.tagId;
    document["tagVersion"] = tagData.tagVersion.c_str();
    JsonArray signatureArray = document["tagSignature"].to<JsonArray>();

    for (size_t i = 0; i < 16; i++) {
        signatureArray.add(tagData.tagSignature[i]);
    }
    

    String requestBody;
    serializeJson(document, requestBody);

    int statusCode = http.POST(requestBody);

    Serial.print("POST status: ");
    Serial.println(statusCode);

    if (statusCode > 0) {
        
        String response = http.getString();

        Serial.print("Response: ");
        Serial.println(response);
    } else {
        Serial.print("Request failed: ");
        Serial.println(http.errorToString(statusCode));
    }

    http.end();

    return statusCode >= 200 && statusCode < 300;
}

std::optional<TagWriteData> getWriteData() {
}