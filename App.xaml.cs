﻿using System.Diagnostics;

namespace Zettelkasten
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
