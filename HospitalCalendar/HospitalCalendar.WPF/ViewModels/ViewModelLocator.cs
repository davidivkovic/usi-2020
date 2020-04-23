using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services;
using HospitalCalendar.Domain.Services.AuthenticationServices;
using HospitalCalendar.EntityFramework;
using HospitalCalendar.EntityFramework.Services;
using HospitalCalendar.EntityFramework.Services.AuthenticationServices;
using HospitalCalendar.WPF.ViewModels.AdministratorMenu;
using HospitalCalendar.WPF.ViewModels.Login;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;


namespace HospitalCalendar.WPF.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            SimpleIoc.Default.Unregister<IMessenger>();
            SimpleIoc.Default.Register<IMessenger>(() => Messenger.Default);

            // Password hasher dependencies
            SimpleIoc.Default.Register<IOptions<PasswordHasherOptions>, OptionsWrapper<PasswordHasherOptions>>();
            SimpleIoc.Default.Register<PasswordHasherOptions>();

            // Password hasher
            SimpleIoc.Default.Register<IPasswordHasher<User>, PasswordHasher<User>>();

            // Register services here
            SimpleIoc.Default.Register<IAuthenticationService, AuthenticationService>();
            SimpleIoc.Default.Register<IUserService, UserService>();
            SimpleIoc.Default.Register<IRoomService, RoomService>();
            SimpleIoc.Default.Register<HospitalCalendarDbContextFactory>();

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<AdministratorViewModel>();
            SimpleIoc.Default.Register<UserRegisterViewModel>();
            SimpleIoc.Default.Register<UserUpdateViewModel>();
            SimpleIoc.Default.Register<RoomCreateViewModel>();
        }

        public MainViewModel MainViewModel => SimpleIoc.Default.GetInstance<MainViewModel>();
        public LoginViewModel LoginViewModel => SimpleIoc.Default.GetInstance<LoginViewModel>();
        public AdministratorViewModel AdministratorViewModel => SimpleIoc.Default.GetInstance<AdministratorViewModel>();
        public UserRegisterViewModel RegistrationViewModel => SimpleIoc.Default.GetInstance<UserRegisterViewModel>();
        public UserUpdateViewModel UserModificationViewModel => SimpleIoc.Default.GetInstance<UserUpdateViewModel>();
        public RoomCreateViewModel RoomCreateViewModel => SimpleIoc.Default.GetInstance<RoomCreateViewModel>();
    }
}
