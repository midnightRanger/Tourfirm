using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;

namespace Tourfirm.API.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[Route("api/paymentinfo")]
[ApiController]
public class PaymentInfoController: ControllerBase
{
    private readonly IPaymentInfo _IPaymentInfo;
    
    public PaymentInfoController(IPaymentInfo iPaymentInfo)
    {
        _IPaymentInfo = iPaymentInfo;
    }

    //get api/paymentinfo
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PaymentInfo>>> Get()
    {
        return await Task.FromResult(await _IPaymentInfo.getPaymentInfos()); 
    }
    
    //get api/paymentinfo/5 
    [HttpGet("{id}")]
    public async Task<ActionResult<PaymentInfo>> Get(int id)
    {
        var paymentinfo = await Task.FromResult(await _IPaymentInfo.getPaymentInfo(id));
        if (paymentinfo == null)
            return NotFound();
        return paymentinfo; 
    }
    
    //post api/paymentinfo
    [HttpPost]
    public async Task<ActionResult<PaymentInfo>> Post(PaymentInfo paymentInfo)
    {
        await _IPaymentInfo.addPaymentInfo(paymentInfo);
        return await Task.FromResult(paymentInfo);
    }
    
    // PUT api/paymentinfo/5
    [HttpPut("{id}")]
    public async Task<ActionResult<PaymentInfo>> Put(int id, PaymentInfo paymentInfo)
    {
        if (id != paymentInfo.Id)
        {
            return BadRequest();
        }
        try
        {
            _IPaymentInfo.updatePaymentInfo(paymentInfo);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PaymentInfoExists(id))
            {
                return NotFound();
            }
            throw;
        }
        return await Task.FromResult(paymentInfo);
    }
    // DELETE api/paymentinfo/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<PaymentInfo>> Delete(int id)
    {
        var paymentInfo = _IPaymentInfo.deletePaymentInfo(id);
        return await Task.FromResult(paymentInfo);
    }
    
    private bool PaymentInfoExists(int id)
    {
        return _IPaymentInfo.checkPaymentInfo(id);
    }
    
    
}
