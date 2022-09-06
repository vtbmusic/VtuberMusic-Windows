using System;

namespace VtuberMusic.AppCore.Models;
public class AppCenterReleasesItem {
    public int id { get; set; }
    public string short_version { get; set; }
    public string version { get; set; }
    public string origin { get; set; }
    public DateTime uploaded_at { get; set; }
    public bool mandatory_update { get; set; }
    public bool enabled { get; set; }
    public bool is_external_build { get; set; }
}
