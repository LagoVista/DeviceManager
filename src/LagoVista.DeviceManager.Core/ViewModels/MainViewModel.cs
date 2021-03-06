﻿using LagoVista.Client.Core.Resources;
using LagoVista.Client.Core.ViewModels;
using LagoVista.Client.Core.ViewModels.Auth;
using LagoVista.Client.Core.ViewModels.Orgs;
using LagoVista.Core.Commanding;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using LagoVista.IoT.DeviceManagement.Core.Models;
using LagoVista.Client.Core.ViewModels.Other;
using System.Threading.Tasks;
using LagoVista.DeviceManager.Core.Resources;

namespace LagoVista.DeviceManager.Core.ViewModels
{
    public class MainViewModel  : ListViewModelBase<IoT.DeviceManagement.Core.Models.DeviceRepositorySummary>
    {
        public MainViewModel()
        {
            MapDeviceRepoCommand = new RelayCommand(MapRepo);

            MenuItems = new List<MenuItem>()
            {                 
                new MenuItem()
                {
                    Command = new RelayCommand(() => ViewModelNavigation.NavigateAsync<UserOrgsViewModel>(this)),
                    Name = ClientResources.MainMenu_SwitchOrgs,
                    FontIconKey = "fa-users"
                },
                new MenuItem()
                {
                    Command = new RelayCommand(() => ViewModelNavigation.NavigateAsync<ChangePasswordViewModel>(this)),
                    Name = ClientResources.MainMenu_ChangePassword,
                    FontIconKey = "fa-key"
                },
                new MenuItem()
                {
                    Command = new RelayCommand(() => ViewModelNavigation.NavigateAsync<InviteUserViewModel>(this)),
                    Name = ClientResources.MainMenu_InviteUser,
                    FontIconKey = "fa-user"
                },
                new MenuItem()
                {
                    Command = new RelayCommand(() => ViewModelNavigation.NavigateAsync<AboutViewModel>(this)),
                    Name = "About",
                    FontIconKey = "fa-info"
                },
                new MenuItem()
                {
                    Command = new RelayCommand(() => Logout()),
                    Name = ClientResources.Common_Logout,
                    FontIconKey = "fa-sign-out"
                }
            };

            ShowIoTAppStudioCommand = new RelayCommand(() => NetworkService.OpenURI(new Uri("https://www.IoTAppStudio.com")));
        }

        private bool _hasRepos;
        public bool HasRepos
        {
            get { return _hasRepos; }
            set { Set(ref _hasRepos, value); }
        }

        private bool _emptyRepos;
        public bool EmptyRepos
        {
            get { return _emptyRepos; }
            set { Set(ref _emptyRepos, value); }
        }

        public async override Task InitAsync()
        {
            await base.InitAsync();

            if (ListItems != null)
            {
                HasRepos = ListItems.Any();
                EmptyRepos = !HasRepos;
            }
        }

        public async void MapRepo(object repoId)
        {
            await NavigateAndViewAsync<DeviceMapViewModel>(repoId.ToString());
        }

        public RelayCommand ShowIoTAppStudioCommand { get; private set; }

        protected override async void ItemSelected(DeviceRepositorySummary model)
        {
            await NavigateAndViewAsync<DeviceExplorerViewModel>(model.Id);
        }

        protected override string GetListURI()
        {
            return $"/api/devicerepos";
        }

        public RelayCommand MapDeviceRepoCommand { get; private set; }
    }
}
