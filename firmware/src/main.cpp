#include <Arduino.h>
#include <WiFi.h>

const char* ssid = "Privat 11";
const char* password = "90118621";

void setup() {
  Serial.begin(115200);
  delay(1000);

  Serial.println("Starting WiFi test...");

  WiFi.mode(WIFI_STA);
  WiFi.begin(ssid, password);

  Serial.print("Connecting");

  while (WiFi.status() != WL_CONNECTED)
  {
    delay(500);
    Serial.print(".");
  }

  Serial.println();
  Serial.println("WiFi connected!");
  Serial.print("ESP32 IP address: ");
  Serial.println(WiFi.localIP());
  
}

void loop() {
  Serial.print("WiFi status: ");
  Serial.println(WiFi.status() == WL_CONNECTED ? "Connected" : "Disconnected");

  delay(10000);
}

/*
#include <Arduino.h>
#include <Wire.h>
#include <Adafruit_PN532.h>

#define SDA_PIN 8
#define SCL_PIN 9

Adafruit_PN532 nfc(SDA_PIN, SCL_PIN);

void setup() {
  Serial.begin(115200);
  
  Serial.println("Starting PN532 I2C test...");

  Wire.begin(SDA_PIN, SCL_PIN);

  nfc.begin();

  uint32_t versiondata = nfc.getFirmwareVersion();

  if (!versiondata)
  {
    Serial.println("Could not find PN532. Check wiring, power, and I2C switch mode.");
    while (true)
    {
      delay(1000);
    }
  }

  Serial.print("Found PN532. Firmware version: ");
  Serial.print((versiondata >> 24) & 0xFF, HEX);
  Serial.print(".");
  Serial.println((versiondata >> 16) & 0xFF, HEX);

  nfc.SAMConfig();

  Serial.println("Waiting for NFC tag...");
}

void loop() {
  uint8_t uid[7];
  uint8_t uidLength;

  bool success = nfc.readPassiveTargetID(PN532_MIFARE_ISO14443A, uid, &uidLength, 1000);

  if (success)
  {
    Serial.println("Tag detected!");

    Serial.print("UID length: ");
    Serial.println(uidLength);

    Serial.print("UID: ");
    for (uint8_t i = 0; i < uidLength; i++)
    {
      if (uid[i] < 0x10)
      {
        Serial.print(0);
      }

      Serial.print(uid[i], HEX);
      Serial.print(" ");
    }

    Serial.println();
    Serial.println("Remove tag...");
    delay(1500);
  }
}
*/
