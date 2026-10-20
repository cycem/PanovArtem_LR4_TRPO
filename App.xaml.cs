using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.Entities;

namespace MassageSlon
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static MassageSalonEntities Context { get; } = new MassageSalonEntities();
        public static Users CurrentUser = null;
    }
}
