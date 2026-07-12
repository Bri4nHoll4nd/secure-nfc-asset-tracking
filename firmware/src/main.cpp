#include <Arduino.h>
#include <Wire.h>
#include <Adafruit_PN532.h>
#include <cstring>

#include "NfcReader.h"
#include "WiFiManager.h"
#include "AppConfig.h"
#include "ApiManager.h"

WiFiManager wifiManager;
NfcReader nfcReader;
ApiManager apiManager(wifiManager);

std::string entityId = "A0-00AA";
std::string entityVersion = "1.0";

const uint8_t signature[16] = {
  0xE7, 0xBA, 0x11, 0x3D,
  0x59, 0xA0, 0x0B, 0x9A,
  0xAC, 0xF6, 0xFD, 0xF4,
  0xDE, 0x8B, 0xCF, 0xAC
};

bool hasWrittenTag = false;

void setup() {
  Serial.begin(115200);
  delay(1000);

  wifiManager.connect();
  nfcReader.start();
}

void loop() {
  if (hasWrittenTag) {
    delay(1000);
    return;
  }

  uint8_t uid[7];
  uint8_t uidLength = 0;
  
  std::optional<TagUidData> tag = nfcReader.readTagUid();

  if (!tag) {
    Serial.println("No tag uid found.");
    return;
  }

  uidLength = tag->tagUidLength;
  std::memcpy(uid, tag->tagUid, sizeof(uidLength));

  Serial.println();
  Serial.println("Writing test data...");

  if (nfcReader.writeDataToTag(tag->tagUid, tag->tagUidLength, entityId, entityVersion, signature)) {
    Serial.println(
      "All test data written successfully"
    );

    hasWrittenTag = true;
  } else {
    Serial.println(
      "Writing test data failed"
    );
  }

  delay(1000);

  /*
  auto tag = nfcReader.readTag();

  if (tag.has_value()) {
    apiManager.sendTagUid(tag.value());
  }
  
  delay(2000);
  */
}
