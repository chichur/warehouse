using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace warehouse
{
    public interface IView
    {
        int[] InputPickets { get; }
        int[][] GetPlatforms();
        event EventHandler<EventArgs> SetCargo;
        event EventHandler<EventArgs> SetPlatforms;

    }

    public class Presenter
    {
        private IView _view;
        
        public Presenter(IView view)
        {
            _view = view;
            _view.SetCargo += new EventHandler<EventArgs>(OnSetCargo);
            _view.SetPlatforms += new EventHandler<EventArgs>(OnSetPlatforms);
        }

        private void OnSetCargo(Object sender, EventArgs e)
        {
            
        }

        private void OnSetPlatforms(Object sender, EventArgs e)
        {
            int [][]
            platformView = _view.GetPlatforms();
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
                            Picket = 132

                        };

                        db.Stocks.Add(stock);
                    }
                }

                db.SaveChanges();
            }
            _view.GetPlatforms();
        }

    }
}

    
