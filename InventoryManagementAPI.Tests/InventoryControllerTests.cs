using InventoryManagementAPI.Controllers;
using InventoryManagementAPI.Models;
using InventoryManagementAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace InventoryManagementAPI.Tests
{
    public class InventoryControllerTests
    {
        private readonly Mock<InventoryService> _serviceMock;
        private readonly InventoryController _controller;

        public InventoryControllerTests()
        {
            _serviceMock = new Mock<InventoryService>(null);
            _controller = new InventoryController(_serviceMock.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfInventories()
        {
            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<Inventory> { new Inventory { Id = 1, Name = "Item1", Quantity = 10 } });

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var items = Assert.IsAssignableFrom<IEnumerable<Inventory>>(okResult.Value);
            Assert.Single(items);
        }

        [Fact]
        public async Task GetById_ReturnsOkResult_WhenItemExists()
        {
            var inventory = new Inventory { Id = 1, Name = "Item1", Quantity = 10 };
            _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(inventory);

            var result = await _controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var item = Assert.IsType<Inventory>(okResult.Value);
            Assert.Equal(1, item.Id);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenItemDoesNotExist()
        {
            _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((Inventory)null);

            var result = await _controller.GetById(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Add_ReturnsCreatedAtAction_WhenModelIsValid()
        {
            var inventory = new Inventory { Id = 1, Name = "Item1", Quantity = 10 };
            _serviceMock.Setup(s => s.AddAsync(inventory)).ReturnsAsync(inventory);
            _controller.ModelState.Clear();

            var result = await _controller.Add(inventory);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var item = Assert.IsType<Inventory>(createdResult.Value);
            Assert.Equal(1, item.Id);
        }

        [Fact]
        public async Task Add_ReturnsBadRequest_WhenModelIsInvalid()
        {
            _controller.ModelState.AddModelError("Name", "Required");
            var inventory = new Inventory();

            var result = await _controller.Add(inventory);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsNoContent_WhenUpdateIsSuccessful()
        {
            var inventory = new Inventory { Id = 1, Name = "Item1", Quantity = 10 };
            _serviceMock.Setup(s => s.UpdateAsync(1, inventory)).ReturnsAsync(true);
            _controller.ModelState.Clear();

            var result = await _controller.Update(1, inventory);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenItemDoesNotExist()
        {
            var inventory = new Inventory { Id = 1, Name = "Item1", Quantity = 10 };
            _serviceMock.Setup(s => s.UpdateAsync(1, inventory)).ReturnsAsync(false);
            _controller.ModelState.Clear();

            var result = await _controller.Update(1, inventory);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenModelIsInvalid()
        {
            _controller.ModelState.AddModelError("Name", "Required");
            var inventory = new Inventory();

            var result = await _controller.Update(1, inventory);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenDeleteIsSuccessful()
        {
            _serviceMock.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

            var result = await _controller.Delete(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenItemDoesNotExist()
        {
            _serviceMock.Setup(s => s.DeleteAsync(1)).ReturnsAsync(false);

            var result = await _controller.Delete(1);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}