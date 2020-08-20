﻿using Staj.Models;
using Staj.Views.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Staj.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            Init();
        }

        void Init()
        {
            BackgroundColor = Constants.BackgroundColor;
            Lbl_Username.TextColor = Constants.MainTextColor;
            Lbl_Password.TextColor = Constants.MainTextColor;
            ActivitySpinner.IsVisible = false;
            LoginIcon.HeightRequest = Constants.LoginIconHeight;
            App.StartCheckIfInternet(lbl_NoInternet, this);

            Entry_Username.Completed += (s, e) => Entry_Password.Focus();
            Entry_Password.Completed += (s, e) => SignInProcedure(s, e);
        }

        async void SignInProcedure(object sender, EventArgs e)
        {
            User user = new User(Entry_Username.Text, Entry_Password.Text);
            if (user.CheckInformation())
            {
                Token token = await App.RestService.Login(user);
                
                //var result = await App.RestService.Login(user);
                //var result = new Token();
                if(token.access_token != null)
                {
                    App.UserDatabase.SaveUser(user);
                    App.TokenDatabase.SaveToken(token);

                    await DisplayAlert("Login", "Login Success","Oke");

                    if (Device.RuntimePlatform.Equals("Android"))
                    {
                        Application.Current.MainPage = new NavigationPage(new MasterDetail());
                    }
                    else if (Device.RuntimePlatform.Equals("iOS"))
                    {
                        await Navigation.PushModalAsync(new NavigationPage(new MasterDetail()));
                    }
                    //else
                    //{
                    //    await Navigation.PushAsync(new Dashboard());
                    //}
                }
            }
            else
            {
                await DisplayAlert("Login", "Login Not Correct, empty username or password", "Oke");
            }
        }

    }
}