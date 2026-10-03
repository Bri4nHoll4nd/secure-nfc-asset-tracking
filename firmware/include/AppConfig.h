#pragma once

#include <cstdint>
#include "LocalConfig.h"

namespace AppConfig {
    constexpr int SDA_PIN = 8;
    constexpr int SCL_PIN = 9;

    constexpr uint8_t ID_BLOCK = 4;
    constexpr uint8_t VERSION_BLOCK = 5;
    constexpr uint8_t SIGNATURE_BLOCK = 6;

    inline constexpr const char* SSID = LocalConfig::WIFI_SSID;

    inline constexpr const char* PASSWORD = LocalConfig::WIFI_PASSWORD;

    inline constexpr const char* API_BASE_URL = LocalConfig::API_BASE_URL;

    inline uint8_t DEFAULT_KEY_A[6] = {
        0xFF, 0xFF, 0xFF,
        0xFF, 0xFF, 0xFF
    };

    inline uint8_t DEFAULT_KEY_B[6] = {
        0xFF, 0xFF, 0xFF,
        0xFF, 0xFF, 0xFF
    };
}