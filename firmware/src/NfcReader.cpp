#include "NfcReader.h"
#include "AppConfig.h"
#include "TagData.h"

#include <Arduino.h>
#include <Wire.h>
#include <Adafruit_PN532.h>
#include <cstdint>
#include <iomanip>
#include <sstream>
#include <string>
#include <cstring>

Adafruit_PN532 nfc(AppConfig::SDA_PIN, AppConfig::SCL_PIN);

bool NfcReader::start() {
    Serial.println("Starting PN532 I2C test...");

    Wire.begin(AppConfig::SDA_PIN, AppConfig::SCL_PIN);

    nfc.begin();

    uint32_t versiondata = nfc.getFirmwareVersion();

    if (!versiondata) {
    Serial.println("Could not find PN532. Check wiring, power, and I2C switch mode.");
    while (true) {
        delay(1000);
        }
    }

    Serial.print("Found PN532. Firmware version: ");
    Serial.print((versiondata >> 24) & 0xFF, HEX);
    Serial.print(".");
    Serial.println((versiondata >> 16) & 0xFF, HEX);

    nfc.SAMConfig();

    Serial.println("Waiting for NFC tag...");

    return true;
}


std::optional<TagUidData> NfcReader::readTagUid() {
    uint8_t uid[7] = {0};
    uint8_t uidLength = 0;
    std::string uidString;

    bool success = nfc.readPassiveTargetID(PN532_MIFARE_ISO14443A, uid, &uidLength, 1000);

    if (!success) {
        return std::nullopt;
    }
    
    
    Serial.println("Tag detected!");

    uidString = uidToString(uid, uidLength);

    Serial.print("UID length: ");
    Serial.println(uidLength);

    Serial.print("UID: ");
    Serial.println(uidString.c_str());

    TagUidData tag;
    tag.tagUidLength = uidLength;

    for (uint8_t i = 0; i < uidLength; i++) {
        tag.tagUid[i] = uid[i];
    }
    
    Serial.println("Remove tag...");
    
    return tag;
}

bool NfcReader::readRawBlock(
        const uint8_t* uid,
        uint8_t uidLength,
        uint8_t blockNumber,
        uint8_t output[16]) {
    
    bool authenticated = nfc.mifareclassic_AuthenticateBlock(
        const_cast<uint8_t*>(uid),
        uidLength,
        blockNumber,
        0,
        AppConfig::DEFAULT_KEY_A
    );

    if (!authenticated) {
        Serial.printf(
            "Authentication failed for block %u\n",
            blockNumber
        );

        return false;
    }

    bool readSuccessful = nfc.mifareclassic_ReadDataBlock(
        blockNumber,
        output
    );

    if (!readSuccessful) {
        Serial.printf(
            "Reading block %u failed\n",
            blockNumber
        );

        return false;
    }
    
    return true;
}

bool NfcReader::readStringBlock(
        const uint8_t* uid,
        uint8_t uidLength,
        uint8_t blockNumber,
        std::string& output) {
    
    uint8_t blockData[16]= {0};

    if (!readRawBlock(
            uid,
            uidLength,
            blockNumber,
            blockData)) {

        return false;
    }
    
    char text[17] = {0};

    std::memcpy(
        text,
        blockData,
        16
    );

    output = text;

    Serial.printf("Read this string: %s\n", output.c_str());

    return true;
}

bool NfcReader::readTagData(
        const uint8_t* uid,
        uint8_t uidLength,
        TagReadData& tagData) {
    
    tagData.tagUid = uidToString(uid, uidLength);
    
    bool idRead = readStringBlock(
        uid,
        uidLength,
        AppConfig::ID_BLOCK,
        tagData.tagId
    );

    if (!idRead) {
        Serial.println("Failed to read id");
        return false;
    }
    
    bool versionRead = readStringBlock(
        uid,
        uidLength,
        AppConfig::VERSION_BLOCK,
        tagData.tagVersion
    );

    if (!versionRead) {
        Serial.println("Failed to read version");
        return false;
    }

    bool signatureRead = readRawBlock(
        uid,
        uidLength,
        AppConfig::SIGNATURE_BLOCK,
        tagData.tagSignature
    );

    if (!signatureRead) {
        Serial.println("Failed to read signature");
        return false;
    }

    return true;
}

bool NfcReader::writeStringToTag(
        const uint8_t* uid,
        uint8_t uidLength,
        uint8_t blockNumber,
        const std::string& value) {
    
    //String length can't be longer than 15 characters, leaving a byte for '\0'
    if (value.length() > 15) {
        Serial.printf(
            "Value for block %u is too long. Maximum is 15 characters.\n",
            blockNumber
        );

        return false;
    }

    uint8_t blockData[16] = {0};

    std::memcpy(
        blockData,
        value.data(),
        value.length()
    );

    Serial.println();

    for (uint8_t i = 0; i < uidLength; i++) {
        Serial.printf("%02X ", uid[i]);
    }

    Serial.println();

    bool authenticated = nfc.mifareclassic_AuthenticateBlock(
        const_cast<uint8_t*>(uid),
        uidLength,
        blockNumber,
        1,
        AppConfig::DEFAULT_KEY_A
    );
    
    if (!authenticated) {
        Serial.printf(
            "Authentication failed for block %u\n",
            blockNumber
        );

        return false;
    }
    
    bool written = nfc.mifareclassic_WriteDataBlock(
        blockNumber,
        blockData
    );

    if (!written) {
        Serial.printf(
            "Writing string to block %u failed\n",
            blockNumber
        );

        return false;
    }
    
    Serial.printf(
        "Successfully wrote '%s' to block %u\n",
        value.c_str(),
        blockNumber
    );

    return true;
}

bool NfcReader::writeSignatureToTag(
        const uint8_t* uid,
        uint8_t uidLength,
        uint8_t blockNumber,
        const uint8_t signature[16]) {

    bool authenticated = nfc.mifareclassic_AuthenticateBlock(
        const_cast<uint8_t*>(uid),
        uidLength,
        blockNumber,
        0,
        AppConfig::DEFAULT_KEY_A
    );
    
    if (!authenticated) {
        Serial.printf(
            "Authentication failed for block %u\n",
            blockNumber
        );

        return false;
    }
    
    bool written = nfc.mifareclassic_WriteDataBlock(
        blockNumber,
        const_cast<uint8_t*>(signature)
    );

    if (!written) {
        Serial.printf(
            "Writing signature to block %u failed\n",
            blockNumber
        );

        return false;
    }
    
    Serial.println("Signature written successfully");
    return true;
}

bool NfcReader::writeDataToTag(
        const uint8_t* uid,
        uint8_t uidLength,
        const std::string& idValue,
        const std::string& versionValue,
        const uint8_t signatureValue[16]) {
        
    bool idWritten = writeStringToTag(
        uid,
        uidLength,
        AppConfig::ID_BLOCK,
        idValue
    );

    if (!idWritten) {
        return false;
    }
    
    bool versionWritten = writeStringToTag(
        uid,
        uidLength,
        AppConfig::VERSION_BLOCK,
        versionValue
    );

    if (!versionWritten) {
        return false;
    }

    bool signatureWritten = writeSignatureToTag(
        uid,
        uidLength,
        AppConfig::SIGNATURE_BLOCK,
        signatureValue
    );

    if (!signatureWritten) {
        return false;
    }

    return true;
}

bool NfcReader::clearBlock(
        const uint8_t* uid,
        uint8_t uidLength,
        uint8_t blockNumber) {
    
    uint8_t emptyData[16] = {0};

    bool authenticated = nfc.mifareclassic_AuthenticateBlock(
        const_cast<uint8_t*>(uid),
        uidLength,
        blockNumber,
        0,
        AppConfig::DEFAULT_KEY_A
    );

    if (!authenticated) {
        Serial.printf(
            "Authentication failed for block %u\n",
            blockNumber
        );

        return false;
    }

    bool cleared = nfc.mifareclassic_WriteDataBlock(
        blockNumber,
        emptyData
    );

    if (!cleared) {
        Serial.printf(
            "Failed to clear block %u\n",
            blockNumber
        );

        return false;
    }

    Serial.printf(
        "Cleared block %u\n",
        blockNumber
    );

    return true;
}

bool NfcReader::clearTagData(
        const uint8_t* uid,
        uint8_t uidLength) {
    
    bool idBlockCleared = clearBlock(
        uid,
        uidLength,
        AppConfig::ID_BLOCK
    );

    bool versionBlockCleared = clearBlock(
        uid,
        uidLength,
        AppConfig::VERSION_BLOCK
    );

    bool signatureBlockCleared = clearBlock(
        uid,
        uidLength,
        AppConfig::SIGNATURE_BLOCK
    );

    return idBlockCleared &&
        versionBlockCleared &&
        signatureBlockCleared;
}

std::string NfcReader::uidToString(const uint8_t* uid, uint8_t uidLength) {
    std::ostringstream output;
    output << std::hex << std::uppercase << std::setfill('0');

    for (std::uint8_t i = 0; i < uidLength; i++) {
        output << std::setw(2) << static_cast<int>(uid[i]);
    }
    
    return output.str();
}