using System;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Net.NetworkInformation;

using Windows.UI.Xaml;

using AppStudio.Services;
using AppStudio.Data;

namespace AppStudio.ViewModels
{
    public class MainViewModel : BindableBase
    {
       private ScienceNewsViewModel _scienceNewsModel;
       private EScienceNewsViewModel _eScienceNewsModel;
       private BbcScienceNewsViewModel _bbcScienceNewsModel;
       private SciencemagViewModel _sciencemagModel;
       private ScienceDailyViewModel _scienceDailyModel;
       private TOIScienceViewModel _tOIScienceModel;
        private PrivacyViewModel _privacyModel;

        private ViewModelBase _selectedItem = null;

        public MainViewModel()
        {
            _selectedItem = ScienceNewsModel;
            _privacyModel = new PrivacyViewModel();

        }
 
        public ScienceNewsViewModel ScienceNewsModel
        {
            get { return _scienceNewsModel ?? (_scienceNewsModel = new ScienceNewsViewModel()); }
        }
 
        public EScienceNewsViewModel EScienceNewsModel
        {
            get { return _eScienceNewsModel ?? (_eScienceNewsModel = new EScienceNewsViewModel()); }
        }
 
        public BbcScienceNewsViewModel BbcScienceNewsModel
        {
            get { return _bbcScienceNewsModel ?? (_bbcScienceNewsModel = new BbcScienceNewsViewModel()); }
        }
 
        public SciencemagViewModel SciencemagModel
        {
            get { return _sciencemagModel ?? (_sciencemagModel = new SciencemagViewModel()); }
        }
 
        public ScienceDailyViewModel ScienceDailyModel
        {
            get { return _scienceDailyModel ?? (_scienceDailyModel = new ScienceDailyViewModel()); }
        }
 
        public TOIScienceViewModel TOIScienceModel
        {
            get { return _tOIScienceModel ?? (_tOIScienceModel = new TOIScienceViewModel()); }
        }

        public void SetViewType(ViewTypes viewType)
        {
            ScienceNewsModel.ViewType = viewType;
            EScienceNewsModel.ViewType = viewType;
            BbcScienceNewsModel.ViewType = viewType;
            SciencemagModel.ViewType = viewType;
            ScienceDailyModel.ViewType = viewType;
            TOIScienceModel.ViewType = viewType;
        }

        public ViewModelBase SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                SetProperty(ref _selectedItem, value);
                UpdateAppBar();
            }
        }

        public Visibility AppBarVisibility
        {
            get
            {
                return SelectedItem == null ? AboutVisibility : SelectedItem.AppBarVisibility;
            }
        }

        public Visibility AboutVisibility
        {

         get { return Visibility.Visible; }
        }

        public void UpdateAppBar()
        {
            OnPropertyChanged("AppBarVisibility");
            OnPropertyChanged("AboutVisibility");
        }

        /// <summary>
        /// Load ViewModel items asynchronous
        /// </summary>
        public async Task LoadData(bool isNetworkAvailable)
        {
            var loadTasks = new Task[]
            { 
                ScienceNewsModel.LoadItems(isNetworkAvailable),
                EScienceNewsModel.LoadItems(isNetworkAvailable),
                BbcScienceNewsModel.LoadItems(isNetworkAvailable),
                SciencemagModel.LoadItems(isNetworkAvailable),
                ScienceDailyModel.LoadItems(isNetworkAvailable),
                TOIScienceModel.LoadItems(isNetworkAvailable),
            };
            await Task.WhenAll(loadTasks);
        }

        /// <summary>
        /// Refresh ViewModel items asynchronous
        /// </summary>
        public async Task RefreshData(bool isNetworkAvailable)
        {
            var refreshTasks = new Task[]
            { 
                ScienceNewsModel.RefreshItems(isNetworkAvailable),
                EScienceNewsModel.RefreshItems(isNetworkAvailable),
                BbcScienceNewsModel.RefreshItems(isNetworkAvailable),
                SciencemagModel.RefreshItems(isNetworkAvailable),
                ScienceDailyModel.RefreshItems(isNetworkAvailable),
                TOIScienceModel.RefreshItems(isNetworkAvailable),
            };
            await Task.WhenAll(refreshTasks);
        }

        //
        //  ViewModel command implementation
        //
        public ICommand RefreshCommand
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    await RefreshData(NetworkInterface.GetIsNetworkAvailable());
                });
            }
        }

        public ICommand AboutCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    NavigationServices.NavigateToPage("AboutThisAppPage");
                });
            }
        }

        public ICommand PrivacyCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    NavigationServices.NavigateTo(_privacyModel.Url);
                });
            }
        }
    }
}
