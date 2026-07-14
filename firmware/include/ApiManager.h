#pragma once

#include "WiFiManager.h"
#include "TagData.h"

#include <optional>

class ApiManager {
    public:
        ApiManager(WiFiManager& manager);
        bool sendTagUid(std::string tagUid);
        bool sendTagData(TagReadData tagData);
        std::optional<TagWriteData> getWriteData();

    private:
        WiFiManager wifiManager;
};