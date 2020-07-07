using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace warehouse
{
    public interface IView
    {
        int?[] InputPickets { set; }
        int?[][] Platforms { get; set; }
        event EventHandler<EventArgs> SetCargo;
        event EventHandler<EventArgs> SetPlatforms;
        event EventHandler<EventArgs> DeletePlatforms;
        event EventHandler<EventArgs> ClearStock;
    }

    public class Presenter
    {
        private IView _view;
        
        public Presenter(IView view)
        {
            _view = view;
            _view.SetCargo += new EventHandler<EventArgs>(OnSetCargo);
            _view.SetPlatforms += new EventHandler<EventArgs>(OnSetPlatforms);
            _view.DeletePlatforms += new EventHandler<EventArgs>(OnDeletePlatforms);
            _view.ClearStock += new EventHandler<EventArgs>(OnClearStock);
            UpdateView();
        }

        private void OnSetCargo(Object sender, EventArgs e)
        {
            //Здесь задаем грузы для площадок
        }

        private void OnSetPlatforms(Object sender, EventArgs e)
        {
            int?[][] platformView = _view.Platforms;
            using (warehousedbContext db = new warehousedbContext())
            {
                for (int i = 0; i < platformView.Length; i++)
                {
                    Platforms platform = new Platforms { Cargo = 6 };
                    for (int j = 0; j < platformView[i].Length; j++)
                    {
                        Stocks stock = new Stocks
                        {
                            NameStock = "Склад 1",
                            IdPlatformNavigation = platform,
                            Picket = platformView[i][j],
                        };

                        db.Stocks.Add(stock);
                    }
                }
                try
                {
                    db.SaveChanges();
                }
                catch(Exception ex)
                {
                    ShowError(ex.Message);
                }  
            }

            UpdateView();
        }

        private void OnClearStock(object sender, EventArgs e)
        {
            using (warehousedbContext db = new warehousedbContext())
            {
                try
                {
                    db.Database.ExecuteSqlCommand("TRUNCATE stocks CASCADE;");
                }
                catch (Exception ex)
                {
                    ShowError(ex.Message);
                }

            }
        }

        private void OnDeletePlatforms(object sender, EventArgs e)
        {

        }

        private void UpdateView()
        {
            using (warehousedbContext db = new warehousedbContext())
            {
                try
                {
                    var pickets = db.Stocks.GroupBy(s => s.Picket)
                        .OrderBy(s => s.Key)
                        .Select(s => s.Key)
                        .ToArray();

                    var platforms = db.Stocks.Where(s => s.NameStock == "Склад 1")
                        .GroupBy(s => s.IdPlatform)
                        .Select(s => s.Key)
                        .ToArray()
                        .Reverse()
                        .ToArray();

                    int?[][] platformView = new int?[platforms.Count()][];
                    for (int i = 0; i < platforms.Count(); i++)
                    {
                        var platformPickets = db.Stocks.Where(s => s.NameStock == "Склад 1")
                            .Where(s => s.IdPlatform == platforms[i])
                            .Select(s => s.Picket)
                            .ToArray();
                            
                        platformView[i] = platformPickets;
                    }

                    _view.InputPickets = pickets;
                    _view.Platforms = platformView;
                }
                catch(IndexOutOfRangeException)
                {
                    ShowError("Данные отсутствуют");
                }
                catch (Exception ex)
                {
                    ShowError(ex.Message);
                }

            }

            
           // _view.InputPickets = 
           // _view.Platforms =
        }

        private static void ShowError(String message)
        {
            MessageBox.Show(
                message,
                "Ошибка базы",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.DefaultDesktopOnly);
        }
    }
}

    
