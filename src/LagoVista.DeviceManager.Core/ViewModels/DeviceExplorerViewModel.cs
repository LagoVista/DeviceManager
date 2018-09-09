using LagoVista.Client.Core.ViewModels;
using LagoVista.Client.Devices;
using LagoVista.Core.Commanding;
using LagoVista.Core.ViewModels;
using LagoVista.DeviceManager.Core.Resources;
using LagoVista.IoT.DeviceAdmin.Models;
using LagoVista.IoT.DeviceManagement.Core.Models;
using System;

namespace LagoVista.DeviceManager.Core.ViewModels
{
    public class DeviceExplorerViewModel : ListViewModelBase<DeviceSummary>
    {
        IDeviceManagementClient _client;

        public DeviceExplorerViewModel(IDeviceManagementClient client)
        {
            AddNewDeviceCommand = new RelayCommand(AddNewDevice);
            SeachNowCommand = new RelayCommand(SearchNow);
            SelectDeviceTypeCommand = new RelayCommand(SelectDeviceType);
          
            _client = client;
        }

        public async void SelectDeviceType()
        {
            await ViewModelNavigation.NavigateAndPickAsync<DeviceTypePickerViewModel>(this, DeviceTypePicked);
        }

        public void SearchNow(Object searchTerm)
        {
            var term = searchTerm;
        }

        public void DeviceTypePicked(object obj)
        {
            if (!(obj is DeviceTypeSummary))
            {
                throw new Exception("Must return DeviceTypeSummary from picker.");
            }

            var deviceTypeSummary = obj as DeviceTypeSummary;
            DeviceType = deviceTypeSummary.Name;
            PerformNetworkOperation(async () =>
            {
                var result = await _client.GetDevicesByDeviceTypeIdAsync(LaunchArgs.ChildId, deviceTypeSummary.Id);
                if (result.Successful)
                {
                    ListItems = result.Model;
                    RaisePropertyChanged(nameof(ListItems));
                }
            });
        }

        public async void AddNewDevice()
        {
            await NavigateAndCreateAsync<ProvisionDeviceViewModel>(LaunchArgs.ChildId);
        }

        protected override void ItemSelected(DeviceSummary model)
        {
            base.ItemSelected(model);
            var launchArgs = new ViewModelLaunchArgs()
            {
                ViewModelType = typeof(MonitorDeviceViewModel),
                LaunchType = LaunchTypes.View
            };

            launchArgs.Parameters.Add(MonitorDeviceViewModel.DeviceRepoId, LaunchArgs.ChildId);
            launchArgs.Parameters.Add(MonitorDeviceViewModel.DeviceId, model.Id);

            ViewModelNavigation.NavigateAsync(launchArgs);
        }

        protected override string GetListURI()
        {
            return $"/api/devices/{LaunchArgs.ChildId}";
        }

        private string _deviceType = DeviceManagerResources.DeviceExplorer_SelectDeviceType_Watermark;
        public string DeviceType
        {
            get { return _deviceType; }
            set { Set(ref _deviceType, value); }
        }

        public RelayCommand AddNewDeviceCommand { get; }
        public RelayCommand SelectDeviceTypeCommand { get; }
        public RelayCommand SeachNowCommand { get; }
    }
}
