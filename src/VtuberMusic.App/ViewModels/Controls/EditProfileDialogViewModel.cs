using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using VtuberMusic.Core.Enums;
using VtuberMusic.Core.Models;
using VtuberMusic.Core.Services;

namespace VtuberMusic.App.ViewModels.Controls;
public partial class EditProfileDialogViewModel : ObservableValidator {
    private IAuthorizationService _authorizationService;
    private IVtuberMusicService _vtuberMusicService;

    [ObservableProperty]
    private Profile profile;

    [ObservableProperty]
    [Required]
    private string nickname;

    [ObservableProperty]
    [Required]
    private ProfileGender genderType;

    [ObservableProperty]
    private DateTimeOffset birthday;

    [ObservableProperty]
    private string signature;

    public Dictionary<string, ProfileGender> GenderTypes = new Dictionary<string, ProfileGender> {
        { "保密", ProfileGender.Unknow },
        { "女", ProfileGender.Woman },
        { "男", ProfileGender.Man }
    };

    public EditProfileDialogViewModel(IAuthorizationService authorizationService, IVtuberMusicService vtuberMusicService) {
        _authorizationService = authorizationService;
        _vtuberMusicService = vtuberMusicService;
        profile = _authorizationService.Profile;

        this.Nickname = profile.nickname;
        this.Signature = profile.signature;
        this.GenderType = profile.gender;
        this.Birthday = profile.birthday;
        ValidateAllProperties();
    }

    [RelayCommand]
    public async Task EditProfile() {
        await _vtuberMusicService.UpdateProfile(this.GenderType, this.Birthday.ToUnixTimeSeconds(), this.Nickname, this.Signature);
        await _authorizationService.AuthorizationAsync();
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e) {
        base.OnPropertyChanged(e);

        ValidateAllProperties();
    }
}
