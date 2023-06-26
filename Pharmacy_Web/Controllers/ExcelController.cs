using System.Collections.Generic;
using static Pharmacy_Web.Controllers.ExcelController;

namespace Pharmacy_Web.Controllers
{
    public class ExcelController : Controller
    {
        private readonly IService<TbItems> _item;
        private readonly IService<TbItemUnits> _itemUnit;
        private readonly IService<TbUnits> _units;
        private readonly IService<TbCategories> _category;
        //  private static List<Drug> randomDrugs = new List<Drug>();
        public ExcelController(IService<TbItemUnits> itemUnit, IService<TbItems> item, IService<TbUnits> units, IService<TbCategories> category)
        {
            _itemUnit = itemUnit;
            _item = item;
            _units = units;
            _category = category;
        }

        public async Task<IActionResult> Index()
        {
          


            //string jsonData = await System.IO.File.ReadAllTextAsync("D:\\4\\Data\\drugs.json");
            //Random random = new Random();
            //List<Drug> drugs = JsonConvert.DeserializeObject<List<Drug>>(jsonData);
            //List<Drug> randomDrugs =
            //     drugs.Where(d => d.id != 0
            //          && !string.IsNullOrEmpty(d.tradename)
            //          && !string.IsNullOrEmpty(d.activeingredient)
            //          && !string.IsNullOrEmpty(d.price)
            //          && !string.IsNullOrEmpty(d.company)
            //          && !string.IsNullOrEmpty(d.composition)
            //          && !string.IsNullOrEmpty(d.pamphlet)
            //          && !string.IsNullOrEmpty(d.group)
            //          && !string.IsNullOrEmpty(d.dosage)).ToList();
            //List<TbCategories> categoriess = randomDrugs
            //.Select(d => d.group)
            //.Distinct()
            //.Select(c => new TbCategories
            //{
            //    Name = c
            //})
            //.ToList();

            //Debugger.Break();
            //var categoriess = await _category.GetAll();
            //List<TbItemUnits> itemUnits = randomDrugs
            //  .Select(a => new TbItemUnits
            //  {
            //      Barcode = string.Empty,
            //      Item = new TbItems
            //      {
            //          ActiveIngredient = a.activeingredient,
            //          Brief = "",
            //          Company = a.company,
            //          Composition = a.composition,
            //          ImageName = $"{a.id}.jpg",
            //          IsActive = true,
            //          Dosage = a.dosage,
            //          IsDeleted = false,
            //          Name = a.tradename,
            //          Pamphlet = a.pamphlet,
            //          CategoryId = categoriess.Where(x => x.Name == a.group).Select(c => c.Id).First()
            //      },
            //      PuchasePrice = Convert.ToDecimal(a.price) - 5,
            //      SalesPrice = Convert.ToDecimal(a.price),
            //      UnitId = 1,
            //      UnitsNumber = 0,
            //      IsActive = true,
            //  })
            //  .ToList();
            //await _itemUnit.AddRange(itemUnits);
            //Debugger.Break();
            return View();
        }
        public class Drug
        {

            public int id { get; set; }
            public string tradename { get; set; }
            public string activeingredient { get; set; }
            public string price { get; set; }
            public string company { get; set; }
            public string group { get; set; }
            public string pamphlet { get; set; }
            public string dosage { get; set; }
            public string composition { get; set; }
        }
    }
}
