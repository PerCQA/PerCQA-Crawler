using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NJBC.DataLayer.IRepository;
using NJBC.DataLayer.Models;
using NJBC.DataLayer.Models.Semeval2015;
using NJBC.DataLayer.Repository;
using NJBC.Models.DTO.Web;
using NJBC.Web.App.Label.Models;

namespace NJBC.Web.App.Label.Controllers
{
    public class LabelController : Controller
    {
        private readonly ISemEvalRepository SemEvalRepository;
        public LabelController(ISemEvalRepository SemEvalRepository)
        {
            this.SemEvalRepository = SemEvalRepository;
        }

        [Route("Label/Label/{id}/{qid?}")]
        public IActionResult Label(string id, string qid)
        {
            LabelVM model = new LabelVM();
            try
            {
                Question res = null;
                if (!string.IsNullOrEmpty(qid) && long.TryParse(qid, out long _qid))
                {
                    res = SemEvalRepository.GetQuestionByQIDAsync(_qid).Result;
                    model.Review = true;
                }
                else
                {
                    res = SemEvalRepository.GetActiveQuestion(id).Result;
                    model.Review = false;
                }
                if (res == null)
                    return Redirect("/");
                model.UserId = res.UserId.Value;
                model.Q = res;
                return View(model);
            }
            catch (Exception ex)
            {
                model.ErrMsg = ex.Message;
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult SetLabel([FromBody]SetLabelCommentParam param)
        {
            var s = SemEvalRepository.SetLabelComment(param).Result;
            if (!s)
                return BadRequest();
            return Json(new { Message = "ثبت شد" });
        }

        [HttpPost]
        public IActionResult NextQuestion([FromBody]CompleteQuestionParam param)
        {
            var s = SemEvalRepository.SetLabelCompelete(param).Result;
            if (!s)
                return BadRequest();
            return Json(new { Message = "ثبت شد" });
        }

        [HttpGet]
        public IActionResult CommentEdit(long id)
        {
            var cm = SemEvalRepository.GetCommentByIdAsync(id).Result;
            return View(cm);
        }

        [HttpPost]
        public IActionResult CommentEdit(CommentEditParam param)
        {
            var res = SemEvalRepository.EditComment(param.CommentId, param.CBodyClean).Result;
            if (res)
                return Redirect("/");
            return View();
        }
    }
}