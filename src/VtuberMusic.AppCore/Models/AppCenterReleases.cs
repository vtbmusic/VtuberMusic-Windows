using System;

namespace VtuberMusic.AppCore.Models;
public class AppCenterReleases {
    public string app_name { get; set; }
    public string app_display_name { get; set; }
    public string app_os { get; set; }
    public string app_icon_url { get; set; }
    public AppCenterOwner owner { get; set; }
    public bool is_external_build { get; set; }
    public string origin { get; set; }
    public int id { get; set; }
    public string version { get; set; }
    public string short_version { get; set; }
    public int size { get; set; }
    public string min_os { get; set; }
    public object device_family { get; set; }
    public string bundle_identifier { get; set; }
    public string fingerprint { get; set; }
    public DateTime uploaded_at { get; set; }
    public string download_url { get; set; }
    public string install_url { get; set; }
    public bool mandatory_update { get; set; }
    public bool enabled { get; set; }
    public string fileExtension { get; set; }
    public bool is_latest { get; set; }
    public string release_notes { get; set; }
    public object is_udid_provisioned { get; set; }
    public object can_resign { get; set; }
    public string[] package_hashes { get; set; }
    public string destination_type { get; set; }
    public string status { get; set; }
    public string distribution_group_id { get; set; }
    public AppCenterDistributionGroups[] distribution_groups { get; set; }
}

public class AppCenterOwner {
    public string name { get; set; }
    public string display_name { get; set; }
}

public class AppCenterDistributionGroups {
    public string id { get; set; }
    public string name { get; set; }
    public string origin { get; set; }
    public string display_name { get; set; }
    public bool is_public { get; set; }
}
