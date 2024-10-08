using AutoMapper;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using RestauranteWeb.Models;
using Service;

namespace RestauranteWeb.Controllers
{
    public class ItemcardapioController : Controller
    {
        private readonly IItemcardapioService itemcardapioService;
        private readonly IMapper mapper;


        

        public ItemcardapioController(IItemcardapioService itemcardapioService, IMapper mapper)
        {
            this.itemcardapioService = itemcardapioService;
            this.mapper = mapper;
        }

        // GET: ItemcardapioController
        public ActionResult Index()
        {
            var listaItemcardapio = itemcardapioService.GetAll();
            var ItemscardapioViewModel = mapper.Map<List<ItemcardapioViewModel>>(listaItemcardapio);
            return View(ItemscardapioViewModel);
        }

        // GET: ItemcardapioController/Details/5
        public ActionResult Details(uint id)
        {
            var itemcardapio = itemcardapioService.Get(id);
                
            var ItemscardapioViewModel = mapper.Map<ItemcardapioViewModel>(itemcardapio);
            return View(ItemscardapioViewModel);
        }

        // GET: ItemcardapioController/Create
        public ActionResult Create()
        {
            var Itemcardapioview = new ItemcardapioViewModel
            {
                IdRestaurante = 1
            };
            return View(Itemcardapioview);
        }

        // POST: ItemcardapioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ItemcardapioViewModel itemcardapioViewModel)
        {
            if (ModelState.IsValid)
            {
                var itemcardapio = mapper.Map<Itemcardapio>(itemcardapioViewModel);
                itemcardapioService.Create(itemcardapio);
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: ItemcardapioController/Edit/5
        public ActionResult Edit(uint id)
        {
            var itemcardapio = itemcardapioService.Get(id);
            var itemcardapioViewModel = mapper.Map<ItemcardapioViewModel>(itemcardapio);
            return View(itemcardapioViewModel);
        }

        // POST: ItemcardapioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(uint id, ItemcardapioViewModel itemcardapioViewModel)
        {
            if (ModelState.IsValid)
            {
                var itemcardapio = mapper.Map<Itemcardapio>(itemcardapioViewModel);
                itemcardapioService.Edit(itemcardapio);
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: ItemcardapioController/Delete/5
        public ActionResult Delete(uint id)
        {
            var itemcardapio = itemcardapioService.Get(id);
            if(itemcardapio == null)
            {
                return NotFound();
            }
            var itemcardapioViewModel = mapper.Map<ItemcardapioViewModel>(itemcardapio);
            return View(itemcardapioViewModel);
        }

        // POST: ItemcardapioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ItemcardapioViewModel itemcardapioViewModel)
        {
            itemcardapioService.Delete(itemcardapioViewModel.Id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ExportarParaExcel()
        {
            var itens = itemcardapioService.GetAll();
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Itens do Cardápio");


                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "Nome";
                worksheet.Cells[1, 3].Value = "Preço";
                worksheet.Cells[1, 4].Value = "Detalhes";
                worksheet.Cells[1, 5].Value = "Ativo";
                worksheet.Cells[1, 6].Value = "Disponível";
                worksheet.Cells[1, 7].Value = "ID Restaurante";
                int row = 2;
                foreach (var item in itens)
                {
                    worksheet.Cells[row, 1].Value = item.Id;
                    worksheet.Cells[row, 2].Value = item.Nome;
                    worksheet.Cells[row, 3].Value = item.Preco;
                    worksheet.Cells[row, 4].Value = item.Detalhes;
                    worksheet.Cells[row, 5].Value = item.Ativo;
                    worksheet.Cells[row, 6].Value = item.Disponivel;
                    worksheet.Cells[row, 7].Value = item.IdRestaurante;
                    row++;
                }
                // Ajustar a largura das colunas
                worksheet.Cells.AutoFitColumns();
                // Prepare o arquivo para download
                var stream = new MemoryStream();
                package.SaveAs(stream);
                var fileName = "ItensDoCardapio.xlsx";
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }

    }
}
