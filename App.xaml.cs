using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MassageSalon.Entities;

namespace MassageSalon
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Entities.MassageSalonEntities1 Context { get; } = new MassageSalonEntities1();
        public static Entities.Users CurrentUser = null;
    }
}
