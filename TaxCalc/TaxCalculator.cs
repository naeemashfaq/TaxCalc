using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;

namespace TaxCalc
{
    public class TaxCalculator:ITaxCalculator
    {
        Dictionary<Commodity, List<Tuple<DateTime, double>>> _customRates = new Dictionary<Commodity, List<Tuple<DateTime, double>>>();
        bool duplicate = false;


        public double GetStandardTaxRate(Commodity commodity)
        {
            if (commodity == Commodity.Default)
                return 0.25;
            if (commodity == Commodity.Alcohol)
                return 0.25;
            if (commodity == Commodity.Food)
                return 0.12;
            if (commodity == Commodity.FoodServices)
                return 0.12;
            if (commodity == Commodity.Literature)
                return 0.6;
            if (commodity == Commodity.Transport)
                return 0.6;
            if (commodity == Commodity.CulturalServices)
                return 0.6;

            return 0.25;
        }

    
        public void SetCustomTaxRate(Commodity commodity, double rate)
        {
            DateTime timeNow = DateTime.Now;
           //if commodity key does not exists, 
            if (!_customRates.ContainsKey(commodity))
                _customRates.Add(commodity, new List<Tuple<DateTime, double>>());
            
            //checking for duplicate values
            foreach (var x in _customRates[commodity])
            {
                if(x.Item1 == timeNow && x.Item2==rate)
                {
                    duplicate = true;
                    break;
                }
            }

            //if time is changed
            SystemEvents.TimeChanged += new EventHandler(SystemEvents_TimeChanged);

            //adding valued to dictionary
            if (duplicate == false)
            {
                _customRates[commodity].Add(Tuple.Create(timeNow, rate));
            }
        }
        
        void SystemEvents_TimeChanged(object sender, EventArgs e)
        {
            duplicate = false;
        }


               
        public double GetTaxRateForDateTime(Commodity commodity, DateTime date)
        {
            List<Tuple<DateTime, double>> cTaxRateList = new List<Tuple<DateTime, double>>();
            int ctr = 1;
           
            //if this key exists then getting the tax rate during specific dates 
            if (_customRates.ContainsKey(commodity))
            {
                cTaxRateList = _customRates[commodity];

                for (int i = 0; i < cTaxRateList.Count - 1; i++)
                {
                    if (date >= cTaxRateList[i].Item1 && date <= cTaxRateList[ctr].Item1)
                    {
                        return cTaxRateList[i].Item2;
                    }
                    ctr++;
                }
            }
            
            return GetStandardTaxRate(commodity);
            
        }

        public double GetCurrentTaxRate(Commodity commodity)
        {
            //if tax is set then return the current tax rate
            if (_customRates.ContainsKey(commodity))
            {
                var cTaxRateList = _customRates[commodity];
                var cTaxRate = cTaxRateList[cTaxRateList.Count - 1].Item2;
                return cTaxRate;
            }
            else
            {
                //if tax rate is not set 
                return GetStandardTaxRate(commodity);
            }
        }
    }

    public enum Commodity
    {
        //PLEASE NOTE: THESE ARE THE ACTUAL TAX RATES THAT SHOULD APPLY, WE JUST GOT THEM FROM THE CLIENT!
        Default,            //25%
        Alcohol,            //25%
        Food,               //12%
        FoodServices,       //12%
        Literature,         //6%
        Transport,          //6%
        CulturalServices    //6%
    }
}






//The focus should be on clean, simple and easy to read code 
//Everything but the public interface may be changed

    

    /// <summary>
    /// Implements a tax calculator for our client.
    /// The calculator has a set of standard tax rates that are hard-coded in the class.
    /// It also allows our client to remotely set new, custom tax rates.
    /// Finally, it allows the fetching of tax rate information for a specific commodity and point in time.
    /// TODO: We know there are a few bugs in the code below, since the calculations look messed up every now and then.
    ///       There are also a number of things that have to be implemented.
    /// </summary>
   
        /// <summary>
        /// Get the standard tax rate for a specific commodity.
        /// </summary>
       


        /// <summary>
        /// This method allows the client to remotely set new custom tax rates.
        /// When they do, we save the commodity/rate information as well as the UTC timestamp of when it was done.
        /// NOTE: Each instance of this object supports a different set of custom rates, since we run one thread per customer.
        /// </summary>
       


        /// <summary>
        /// Gets the tax rate that is active for a specific point in time (in UTC).
        /// A custom tax rate is seen as the currently active rate for a period from its starting timestamp until a new custom rate is set.
        /// If there is no custom tax rate for the specified date, use the standard tax rate.
        /// </summary>
       

        /// <summary>
        /// Gets the tax rate that is active for the current point in time.
        /// A custom tax rate is seen as the currently active rate for a period from its starting timestamp until a new custom rate is set.
        /// If there is no custom tax currently active, use the standard tax rate.
        /// </summary>
       

  
