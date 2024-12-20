﻿using AutoMapper;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RestauranteWeb.Models;

namespace RestauranteWeb.Controllers
{
    public class MesaController : Controller
    {
        private readonly IMesaService mesa;
        private readonly IMapper mapper;
        private readonly IRestauranteService restauranteService;

        public MesaController(IMesaService mesa, IMapper mapper, IRestauranteService restauranteService)
        {
            this.mesa = mesa;
            this.mapper = mapper;
            this.restauranteService = restauranteService;
        }
        // GET: MesaController
        public ActionResult Index()
        {
            var listaMesa = mesa.GetDtos();
            var MesaViewModel = mapper.Map<List<MesaViewModel>>(listaMesa);
            return View(MesaViewModel);
        }
        // GET: MesaController/Create
        public ActionResult Create()
        {
            var Mesaview = new MesaViewModel();
            var Restaurantes = restauranteService.GetDtos();
            return View(Mesaview);
        }
        // POST: MesaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MesaViewModel mesaViewModel)
        {
            if (ModelState.IsValid)
            {
                var Mesa = mapper.Map<Mesa>(mesaViewModel);
                mesa.Create(Mesa);
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: MesaController/Edit/5
        public ActionResult Edit(int id)
        {
            var Mesa = mesa.Get(id);
            var MesaViewModel = mapper.Map<MesaViewModel>(Mesa);
            var Restaurantes = restauranteService.GetDtos();
            MesaViewModel.Restaurantes = new SelectList(Restaurantes, "Id", "Nome", Mesa.IdRestaurante);
            return View(MesaViewModel);
        }

        // POST:MesaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, MesaViewModel mesaViewModel)
        {
            if (ModelState.IsValid)
            {
                var mesa1 = mapper.Map<Mesa>(mesaViewModel);
                mesa.Edit(mesa1);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: MesaController/Delete/5
        public ActionResult Delete(int id)
        {
            var Mesa = mesa.GetById(id).FirstOrDefault();
            var mesaViewModel1 = mapper.Map<MesaViewModel>(Mesa);
            return View(mesaViewModel1);
        }

        // POST: MesaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, MesaViewModel mesa2)
        {
            mesa.Delete(mesa2.Id);
            return RedirectToAction(nameof(Index));
        }
    }
}

