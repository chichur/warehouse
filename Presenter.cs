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
        Dictionary<int?[], int?> Cargo { get; set; }
        event EventHandler<EventArgs> SetCargo;
        event EventHandler<EventArgs> SetPlatforms;
        event EventHandler<EventArgs> DeletePlatforms;
        event EventHandler<EventArgs> GetHistory;
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
            _view.GetHistory += new EventHandler<EventArgs>(OnGetHistory);
            UpdateView();
        }

        private void OnSetCargo(Object sender, EventArgs e)
        {
            Dictionary<int?[], int?> pairs_picket_cargo = _view.Cargo;
            using (warehousedbContext db = new warehousedbContext())
            {
                foreach (int?[] key in pairs_picket_cargo.Keys)
                {
                    var platform = db.Platforms.Where(p => p.IdPlatform == db.Stocks.Where(s => s.NameStock == "Склад 1")
                            .Where(s => s.Picket == key[0])
                            .Select(s => s.IdPlatform)
                            .First()).First();

                    platform.Cargo = pairs_picket_cargo[key];
                }
                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    ShowError(ex.Message);
                }
            }

            UpdateView();
        }

        private void OnSetPlatforms(Object sender, EventArgs e)
        {
            int?[][] platformView = _view.Platforms;
            using (warehousedbContext db = new warehousedbContext())
            {
                for (int i = 0; i < platformView.Length; i++)
                {
                    Platforms platform = new Platforms { Cargo = 0 };
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

        private void OnDeletePlatforms(object sender, EventArgs e)
        {
            int?[][] platformView = _view.Platforms;
            using (warehousedbContext db = new warehousedbContext())
            {
                if (platformView != null)
                    for (int i = 0; i < platformView.Length; i++)
                    {
                        var platform = db.Platforms.Where(p => p.IdPlatform == db.Stocks.Where(s => s.NameStock == "Склад 1")
                                .Where(s => s.Picket == platformView[i][0])
                                .Select(s => s.IdPlatform)
                                .First()).First();

                        if (platform != null)
                            db.Platforms.Remove(platform);
                    }
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        ShowError(ex.Message);
                    }
            }
        }

        private void OnGetHistory(object sender, EventArgs e)
        {
            using (warehousedbContext db = new warehousedbContext())
            {
                try
                {
                    var q1 = db.Stocks.Where()
                    _view.InputPickets = pickets;
                    _view.Platforms = platformView;
                    _view.Cargo = pairs_platformid_cargo;
                }
                catch (IndexOutOfRangeException)
                {
                    ShowError("Данные отсутствуют");
                }
                catch (Exception ex)
                {
                    ShowError(ex.Message);
                }
            }
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
                    // создание словаря пара груз платформы и список пикетов
                    Dictionary<int?[], int?> pairs_platformid_cargo = new Dictionary<int?[], int?>();

                    var platformForTable = db.Platforms.Select(p => new { p.IdPlatform, p.Cargo }).ToList() ;
                    foreach (var cargo in platformForTable)
                    {
                        var list_pickets = db.Stocks.Where(s => s.NameStock == "Склад 1")
                            .Where(s => s.IdPlatform == cargo.IdPlatform)
                            .Select(s => s.Picket)
                            .ToArray();

                        pairs_platformid_cargo.Add(list_pickets, cargo.Cargo);
                    }
                        
                        //закончил здесь







                    _view.InputPickets = pickets;
                    _view.Platforms = platformView;
                    _view.Cargo = pairs_platformid_cargo;
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

    
