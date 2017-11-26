using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LinFx.Data.Extensions;
using LinFx.Utils;
using LinFx.Domain.Entities;
using LinFx.Domain.Services;

namespace LinFx.SaaS.Web.Controllers
{
    public class LinFxController<TEntity, TService> : LinFxController
        where TEntity : IEntity<long>
        where TService : IDataService<TEntity, long>, new()
    {
        protected TService _service = new TService();

        [HttpGet]
        public virtual IActionResult Get(string filter = "", Paging paging = null, string sortby = "")
        {
            try
            {
                var dic = UriUtils.ToFilter(filter);
                var sorting = UriUtils.ToSorting(sortby);
                var result = _service.GetList(dic, paging, sorting);
                return Json(new
                {
                    result.Total,
                    result.Count,
                    result.Items,
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public virtual IActionResult Get(long id)
        {
            try
            {
                var item = _service.Get(id);
                if (item == null)
                    return NotFound();
                return Ok(item);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        public virtual IActionResult Post([FromBody]TEntity item)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                _service.Insert(item);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public virtual IActionResult Put(long id, [FromBody]TEntity item)
        {
            try
            {
                item.Id = id;
                _service.Update(item);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public virtual IActionResult Delete(long id)
        {
            try
            {
                _service.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

    public class LinFxController : Controller
    {
    }
}
