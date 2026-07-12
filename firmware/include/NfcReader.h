#pragma once

#include "TagData.h"
#include "ApiManager.h"

#include <Adafruit_PN532.h>
#include <optional>
#include <string>
#include <cstdint>

class NfcReader {
    public:
        //Initialize PN532
        bool start();

        //Read tag data and store it
        std::optional<TagUidData> readTagUid();

        std::optional<TagReadData> readTagData();

        //Write string to tag
        bool writeStringToTag(
            const uint8_t* uid,
            uint8_t uidLength,
            uint8_t blockNumber,
            const std::string& value);

        //Write byte to tag
        bool writeSignatureToTag(
            const uint8_t* uid,
            uint8_t uidLength,
            uint8_t blockNumber,
            const uint8_t signatureValue[16]);

        bool writeDataToTag(
            const uint8_t* uid,
            uint8_t uidLength,
            const std::string& idValue,
            const std::string& versionValue,
            const uint8_t signatureValue[16]);

        bool clearBlock(
            const uint8_t* uid,
            uint8_t uidLength,
            uint8_t blockNumber);

        bool clearTagData(
            const uint8_t* uid,
            uint8_t uidLength);

    private:
        std::string uidToString(std::uint8_t* uid, std::uint8_t uidLength);
};