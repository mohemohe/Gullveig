using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Gullveig.Test
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            var menuItem = new List<NotifyIconMenu>();
            menuItem.Add(new NotifyIconMenu("バルーンテスト", () => this.ShowNotifyBaloon("クリック", "テストアイテムがクリックされました")));
            menuItem.Add(new NotifyIconMenu("何もしない", null));
            SetNotifyIconMenu("終了", menuItem);

            Loaded += (s, a) =>
            {
                new Action(async () =>
                {
                    await Task.Run(() => System.Threading.Thread.Sleep(3000));
                    for (var i = 3; i > 0; i--)
                    {
                        StatusBar.Text = i.ToString();
                        await Task.Run(() => System.Threading.Thread.Sleep(1000));
                    }
                    
                    StatusBar.Text = "ババーン";
                })();
            };
        }
    }
}
