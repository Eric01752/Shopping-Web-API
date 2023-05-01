using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Interfaces;
using ShoppingApp.Models;
using ShoppingApp.Models.Dto;
using ShoppingApp.Repository;

namespace ShoppingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public OrderController(IOrderRepository orderRepository, ICustomerRepository customerRepository, IProductRepository productRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetOrders()
        {
            var orders = _mapper.Map<List<OrderDto>>(_orderRepository.GetOrders());

            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }

            return Ok(orders);
        }

        [HttpGet("{id}")]
        public IActionResult GetOrder(int id)
        {
            if (!_orderRepository.OrderExists(id))
            {
                return NotFound();
            }

            var order = _mapper.Map<OrderDto>(_orderRepository.GetOrder(id));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(order);
        }

        [HttpGet("orderItems/{id}")]
        public IActionResult GetOrderItems(int id)
        {
            var orderItems = _orderRepository.GetOrderItems(id);

            List<OrderItemsOutboundDto> orderItemsOutboundDtos = new List<OrderItemsOutboundDto>();

            foreach (var item in orderItems)
            {
                var orderItemOutboundDto = new OrderItemsOutboundDto()
                {
                    Product = _mapper.Map<ProductDto>(item.Product),
                    Quantity = item.Quantity,
                    Price = item.Price
                };

                orderItemsOutboundDtos.Add(orderItemOutboundDto);
            }

            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }

            return Ok(orderItemsOutboundDtos);
        }

        [HttpPost]
        public IActionResult CreateOrder([FromQuery] int customerId, [FromBody] OrderDto orderCreate)
        {
            if (orderCreate == null)
            {
                return BadRequest(ModelState);
            }

            var order = _orderRepository.GetOrders().FirstOrDefault(c => c.Id == orderCreate.Id);

            if (order != null)
            {
                ModelState.AddModelError("", "Order already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var orderMap = _mapper.Map<Order>(orderCreate);

            orderMap.Customer = _customerRepository.GetCustomer(customerId);

            if (!_orderRepository.CreateOrder(orderMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPost("orderItems")]
        public IActionResult CreateOrderItems([FromBody] IEnumerable<OrderItemsInboundDto> orderItemsCreate)
        {
            if (orderItemsCreate == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<OrderItems> orderItemsMap = new List<OrderItems>();

            foreach (var orderItem in orderItemsCreate)
            {
                orderItemsMap.Add(_mapper.Map<OrderItems>(orderItem));
            }

            if (!_orderRepository.CreateOrderItems(orderItemsMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{orderId}")]
        public IActionResult UpdateOrder(int orderId, [FromQuery] int customerId, [FromBody] OrderDto updatedOrder)
        {
            if (updatedOrder == null)
            {
                return BadRequest(ModelState);
            }

            if (orderId != updatedOrder.Id)
            {
                return BadRequest(ModelState);
            }

            if (!_orderRepository.OrderExists(orderId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var orderMap = _mapper.Map<Order>(updatedOrder);

            orderMap.Customer = _customerRepository.GetCustomer(customerId);

            if (!_orderRepository.UpdateOrder(orderMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpPut("orderItems/{orderId}")]
        public IActionResult UpdateOrderItems(int orderId, [FromBody] IEnumerable<OrderItemsInboundDto> updatedOrderItems)
        {
            if (updatedOrderItems == null)
            {
                return BadRequest(ModelState);
            }

            foreach (var orderItem in updatedOrderItems)
            {
                if (orderId != orderItem.OrderId)
                {
                    return BadRequest(ModelState);
                }
            }

            if (!_orderRepository.OrderExists(orderId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<OrderItems> orderItemsMap = new List<OrderItems>();

            foreach (var orderItem in updatedOrderItems)
            {
                var tempOrderItem = _mapper.Map<OrderItems>(orderItem);

                tempOrderItem.Product = _productRepository.GetProduct(orderItem.ProductId);

                orderItemsMap.Add(tempOrderItem);
            }

            if (!_orderRepository.UpdateOrderItems(orderItemsMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{orderId}")]
        public IActionResult DeleteOrder(int orderId)
        {
            if (!_orderRepository.OrderExists(orderId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var orderToDelete = _orderRepository.GetOrder(orderId);

            if (!_orderRepository.DeleteOrder(orderToDelete))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("orderItems/{orderId}")]
        public IActionResult DeleteOrderItems(int orderId)
        {
            if (!_orderRepository.OrderExists(orderId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var orderItemsToDelete = _orderRepository.GetOrderItems(orderId);

            if (!_orderRepository.DeleteOrderItems(orderItemsToDelete))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
