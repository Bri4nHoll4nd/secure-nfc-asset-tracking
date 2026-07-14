#pragma once

#include <array>
#include <cstdint>

struct TagWriteData {
    std::string tagId;
    std::string tagVersion;
    std::string tagSignature;
};

struct TagReadData {
    std::string tagUid;
    std::string tagId;
    std::string tagVersion;
    uint8_t tagSignature[16] = {0};
};

struct TagUidData {
    uint8_t tagUid[7] = {0};
    uint8_t tagUidLength = 0;
};