using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Stateless;

namespace FakeXiecheng.API.Models
{
    public enum OrderStateEnum
    {
        Pending,//订单已生成，待办订单
        Processing,//支付处理中
        Completed,//交易成功
        Declined,//交易失败
        Cancelled,//订单取消后
        refund//已退款
    }
    public enum OrderStateTriggerEnum
    {
        PlaceOrder, //支付
        Approve, //支付成功
        Reject, // 支付失败
        Cancel, //取消
        Return //退货
    }
    public class Order
    {
        public Order()
        {
            StateMachineInit(); 
        }
        [Key]
        public Guid Id { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<LineItem> OrderItems { get; set; }
        public OrderStateEnum State { get; set; }
        public DateTime CreateDateUTC { get; set; }
        //第三方支付信息
        public string TransactionMetadata { get; set; }
        StateMachine<OrderStateEnum, OrderStateTriggerEnum> _machine;
        private void StateMachineInit()
        {
            _machine = new StateMachine<OrderStateEnum, OrderStateTriggerEnum>(OrderStateEnum.Pending);
            //待定订单。支付，则处理中； 取消，则取消的；
            _machine.Configure(OrderStateEnum.Pending)
                .Permit(OrderStateTriggerEnum.PlaceOrder, OrderStateEnum.Processing)
                .Permit(OrderStateTriggerEnum.Cancel, OrderStateEnum.Cancelled);
            //支付处理中。支付成功，则完成的；支付失败，则失败的；
            _machine.Configure(OrderStateEnum.Processing)
                .Permit(OrderStateTriggerEnum.Approve, OrderStateEnum.Completed)
                .Permit(OrderStateTriggerEnum.Reject, OrderStateEnum.Declined);
            //支付失败的。支付，则处理中；       取消，则取消的；
            _machine.Configure(OrderStateEnum.Declined)
                .Permit(OrderStateTriggerEnum.PlaceOrder, OrderStateEnum.Processing);
            //.Permit(OrderStateTriggerEnum.Cancel, OrderStateEnum.Cancelled);
            //支付完成的，退货， 则退货的
            _machine.Configure(OrderStateEnum.Completed)
                .Permit(OrderStateTriggerEnum.Return, OrderStateEnum.refund);
        }

    }
}
