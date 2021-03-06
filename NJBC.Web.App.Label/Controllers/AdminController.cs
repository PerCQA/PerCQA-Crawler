using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NJBC.DataLayer.IRepository;
using Microsoft.AspNetCore.Mvc;
using NJBC.DataLayer.Models.Semeval2015;
using NJBC.Web.App.Label.Models;
using NJBC.Models.DTO.Web;
using NJBC.Common;

namespace NJBC.Web.App.Label.Controllers
{
    public class AdminController : Controller
    {
        private readonly ISemEvalRepository SemEvalRepository;
        public AdminController(ISemEvalRepository SemEvalRepository)
        {
            this.SemEvalRepository = SemEvalRepository;
        }

        [HttpGet]
        public IActionResult Detail()
        {
            DatailVM model = SemEvalRepository.GetDetailData().Result;
            return View(model);
        }

        [HttpGet]
        public IActionResult QuestionsToken()
        {
            ViewBag.token = "0";
            return View();
        }

        [HttpPost]
        public IActionResult QuestionsToken(string username, string password)
        {
            if (SemEvalRepository.Auth(username, password).Result)
            {
                ViewBag.token = token;
                return View();
            }
            ViewBag.token = "0";
            return View();
        }

        [Route("Admin/Questions/{id}/{page?}")]
        public IActionResult Questions(string id, int page = 1)
        {
            QuestionsVM model = new QuestionsVM();
            if (id == token)
            {
                model.Page = page - 1;
                model.Count = 1000;
                model.Questions = SemEvalRepository.GetQuestionList(model.Count, page).Result;
                model.Max = SemEvalRepository.GetQuestionsCount().Result;
                model.Token = id;
                return View(model);
            }
            return Redirect("/");
        }

        [HttpPost]
        public IActionResult RejectQuestion(int id)
        {
            var s = SemEvalRepository.RejectQuestion(id).Result;
            if (!s)
                return BadRequest();
            return Ok("ok");
        }

        [HttpPost]
        public IActionResult ActiveQuestion(int id)
        {
            var s = SemEvalRepository.ActiveQuestion(id).Result;
            if (!s)
                return BadRequest();
            return Ok("ok");
        }

        [HttpPost]
        public IActionResult AdvQuestion(int id)
        {
            var s = SemEvalRepository.AdvQuestion(id).Result;
            if (!s)
                return BadRequest();
            return Ok("ok");
        }

        private string token
        {
            get
            {
                int hour = DateTime.Now.Hour + 1;

                if (hour % 2 == 0)
                    hour *= 13;
                else
                    hour *= 15;

                return (DateTime.Now.Day * DateTime.Now.Month * hour + hour).ToString();
            }
        }


        [HttpGet]
        public IActionResult QuestionEdit(long id)
        {
            QuestionEditParam model = new QuestionEditParam();
            var _model = SemEvalRepository.GetQuestionByIdAsync(id).Result;
            model.QBody = _model.QBody;
            model.QSubject = _model.QSubject;
            model.QuestionId = _model.QuestionId;
            using (TextHelper textHelper = new TextHelper())
            {
                model.QSubjectClean = textHelper.CleanReview(_model.QSubject);
                model.QBodyClean = textHelper.CleanReview(_model.QBody);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult QuestionEdit(QuestionEditParam param)
        {
            var res = SemEvalRepository.EditQuestion(param).Result;
            if (res)
                return Redirect("/");
            return View();
        }
    }
}