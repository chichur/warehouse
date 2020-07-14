using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using warehouse.Models;

namespace warehouse
{
    // Простейшая реализация паттерна MVP 
    // интерфейс для обмена представления и представителя, который мы должны обязательно
    // реализовать в форме 
    public interface IView
    {
        int?[] InputPickets { set; } // свойство для ввода номеров пикетов
        int?[][] Platforms { get; set; } // свойство для 
        Dictionary<int?[], int?> Cargo { get; set; }
        event EventHandler<EventArgs> SetCargo;
        event EventHandler<EventArgs> SetPlatforms;
        event EventHandler<EventArgs> DeletePlatforms;
    }

    // класс представителя, поле которого интерфейс 
    public class Presenter
    {
        // поле класса является интерфейсом IView
        private IView _view;
        
        public Presenter(IView view)
        {
            // инициализация экземпляра
            _view = view; // пробрасываем наше представление

            // подписываемся на события
            _view.SetCargo += new EventHandler<EventArgs>(OnSetCargo); 
            _view.SetPlatforms += new EventHandler<EventArgs>(OnSetPlatforms);
            _view.DeletePlatforms += new EventHandler<EventArgs>(OnDeletePlatforms);

            // обновление представления
            UpdateView(); 
        }

        // обработчик события установки груза
        private void OnSetCargo(Object sender, EventArgs e)
        {
            // получаем от формы данные о грузе с помощью свойства 
            Dictionary<int?[], int?> pairs_picket_cargo = _view.Cargo;

            // подключаемся к базе 
            using (warehousedbContext db = new warehousedbContext())
            {
                // проходим по всем ключам, ключ - список пикетов
                foreach (int?[] key in pairs_picket_cargo.Keys)
                {
                    // запрос к таблице площадки, выборка Склада 1 где мы получаем первый элемент из таблицы площадок
                    // по первому пикету из списка используя под запрос из таблицы "склады"
                    var platform = db.Platforms.Where(p => p.IdPlatform == db.Stocks.Where(s => s.NameStock == "Склад 1")
                            .Where(s => s.Picket == key[0])
                            .Select(s => s.IdPlatform)
                            .First()).First();

                    // обновляем запись о грузе
                    platform.Cargo = pairs_picket_cargo[key];
                }
                try
                {
                    // обработка исключений
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    // вывод сообщения исключения
                    ShowError(ex.Message);
                }
            }

            // обновление вида
            UpdateView();
        }

        // обработчик события установки площадок
        private void OnSetPlatforms(Object sender, EventArgs e)
        {

            int?[][] platformView = _view.Platforms;
            using (warehousedbContext db = new warehousedbContext())
            {
                for (int i = 0; i < platformView.Length; i++)
                {
                    // иницилизируем запись для каждого пикета 
                    Platforms platform = new Platforms { Cargo = 0 }; // начальный груз 0
                    for (int j = 0; j < platformView[i].Length; j++)
                    {
                        Stocks stock = new Stocks
                        {
                            NameStock = "Склад 1",
                            IdPlatformNavigation = platform, // присваиваем ссылку на площадку
                            Picket = platformView[i][j],
                        };

                        db.Stocks.Add(stock); // добавляем в контекст
                    }
                }
                try
                {
                    db.SaveChanges(); // сохраняем изменения в бд
                }
                catch(Exception ex)
                {
                    ShowError(ex.Message);
                }  
            }

            UpdateView();
        }

        // обработчик события удаления платформ
        private void OnDeletePlatforms(object sender, EventArgs e)
        {
            int?[][] platformView = _view.Platforms;
            using (warehousedbContext db = new warehousedbContext())
            {
                if (platformView != null) // проверка на пустое значение
                    for (int i = 0; i < platformView.Length; i++)
                    {
                        // запрос аналогичный в OnSetCargo
                        var platform = db.Platforms.Where(p => p.IdPlatform == db.Stocks.Where(s => s.NameStock == "Склад 1")
                                .Where(s => s.Picket == platformView[i][0])
                                .Select(s => s.IdPlatform)
                                .First()).First();

                        if (platform != null)
                            db.Platforms.Remove(platform); // удаляем из контекста
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

        // функция обновления вида
        private void UpdateView()
        {
            using (warehousedbContext db = new warehousedbContext())
            {
                try
                {
                    // получить номера пикетов
                    var pickets = db.Stocks.GroupBy(s => s.Picket)
                        .OrderBy(s => s.Key)
                        .Select(s => s.Key)
                        .ToArray();

                    // получить и сгрупировать по площадками записи с таблицы склады
                    var platforms = db.Stocks.Where(s => s.NameStock == "Склад 1")
                        .GroupBy(s => s.IdPlatform)
                        .Select(s => s.Key)
                        .ToArray()
                        .Reverse()
                        .ToArray();
                    
                    // сформировать данные для представления
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

                    // получить данные для таблицы грузов
                    var platformForTable = db.Platforms.Select(p => new { p.IdPlatform, p.Cargo }).ToList() ;
                    foreach (var cargo in platformForTable)
                    {
                        var list_pickets = db.Stocks.Where(s => s.NameStock == "Склад 1")
                            .Where(s => s.IdPlatform == cargo.IdPlatform)
                            .Select(s => s.Picket)
                            .ToArray();

                        pairs_platformid_cargo.Add(list_pickets, cargo.Cargo);
                    }

                    // присвоить значения свойствам представления (для обновления)
                    _view.InputPickets = pickets;
                    _view.Platforms = platformView;
                    _view.Cargo = pairs_platformid_cargo;
                }
                catch(IndexOutOfRangeException)
                {
                    ShowError("Данные отсутствуют"); // вызывается если количество пикетов равно нулю
                }
                catch (Exception ex)
                {
                    ShowError(ex.Message);
                }
            }
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

    
