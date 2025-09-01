using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trucks.Api.Controllers;
using Trucks.Api.Dtos;
using Trucks.Api.Interfaces;
using Trucks.Api.Models;
using Trucks.Api.Services;
using Xunit;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Trucks.Api.Tests
{
    public class TrucksControllerTests
    {
        private readonly Mock<ITruckService> _mockService;
        private readonly TrucksController _controller;


        public TrucksControllerTests()
        {
            // Mock do TruckService
            _mockService = new Mock<ITruckService>(); 
            _controller = new TrucksController(_mockService.Object);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenTruckExists()
        {
            // Arrange
            var truck = new Truck { Id = 1, Model = TruckModel.FH, ManufactureYear = 2022, ChassisId = "ABC123", Color = "Azul", Plant = Plant.Brasil };
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(truck);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            var dto = okResult!.Value as TruckDto;
            dto!.ChassisId.Should().Be("ABC123");
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenTruckDoesNotExist()
        {
            _mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((Truck?)null);

            var result = await _controller.GetById(99);

            result.Result.Should().BeOfType<NotFoundResult>();
        }
        
        [Fact]
        public async Task GetByChassis_ShouldReturnTruck_WhenFound()
        {
            // Arrange
            var trucks = new[]
            {
                new Truck { Id = 1, Model = TruckModel.VM, ManufactureYear = 2019, ChassisId = "CH123", Color = "White", Plant = Plant.Suécia }
            };

            _mockService.Setup(s => s.GetByChassisAsync("CH123")).ReturnsAsync(trucks);

            // Act
            var result = await _controller.GetByChassis("CH123");
            var okResult = result.Result as OkObjectResult;
            var value = okResult!.Value as IEnumerable<TruckDto>;

            // Assert
            okResult.Should().NotBeNull();
            value.Should().ContainSingle();
            value.First().ChassisId.Should().Be("CH123");
        }

        [Fact]
        public async Task GetByChassis_ShouldReturnNull_WhenNotFound()
        {
            // Arrange
            _mockService.Setup(s => s.GetByChassisAsync("NOTFOUND")).ReturnsAsync(System.Array.Empty<Truck>());

            // Act
            var result = await _controller.GetByChassis("NOTFOUND");

            // Assert
            result?.Result.Should().BeNull();
        }
        
        [Fact]
        public async Task GetAll_ShouldReturnListOfTrucks()
        {
            // Arrange
            var trucks = new List<Truck>
            {
                new Truck { Id = 1, Model = TruckModel.FM, ManufactureYear = 2019, ChassisId = "ABC123", Color = "Blue", Plant = Plant.USA },
                new Truck { Id = 2, Model = TruckModel.VM, ManufactureYear = 2020, ChassisId = "XYZ789", Color = "Red", Plant = Plant.França }
            };

            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(trucks);

            // Act
            var result = await _controller.GetAll();
            var okResult = result.Result as OkObjectResult;
            var value = okResult!.Value as IEnumerable<TruckDto>;

            // Assert
            okResult.Should().NotBeNull();
            value.Should().HaveCount(2);
            value.First().ChassisId.Should().Be("ABC123");
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedTruck()
        {
            // Arrange
            var dto = new TruckCreateDto
            {
                Model = TruckModel.FM,
                ManufactureYear = 2023,
                ChassisId = "XYZ789",
                Color = "Preto",
                Plant = Plant.Suécia
            };

            var createdTruck = new Truck { Id = 10, Model = dto.Model, ManufactureYear = dto.ManufactureYear, ChassisId = dto.ChassisId, Color = dto.Color, Plant = dto.Plant };

            _mockService.Setup(s => s.CreateAsync(It.IsAny<Truck>()))
                        .ReturnsAsync(createdTruck);

            // Act
            var result = await _controller.Create(dto);

            // Assert
            var createdAt = result.Result as CreatedAtActionResult;
            createdAt.Should().NotBeNull();
            var returnDto = createdAt!.Value as TruckDto;
            returnDto!.Id.Should().Be(10);
            returnDto.ChassisId.Should().Be("XYZ789");
        }

        [Fact]
        public async Task Create_ShouldReturnConflict_WhenChassisAlreadyExists()
        {
            // Arrange
            var truck = new TruckCreateDto
            {
                Model = TruckModel.VM,
                ManufactureYear = 2020,
                ChassisId = "DUPLICATE",
                Color = "Red",
                Plant = Plant.Brasil
            };

            // Simula um erro de duplicidade de chave no banco
            _mockService.Setup(s => s.CreateAsync(It.IsAny<Truck>()))
                        .ThrowsAsync(new DbUpdateException("Duplicate key error"));

            // Act
            var result = await _controller.Create(truck);

            // Assert
            var conflict = result.Result as ConflictObjectResult;
            conflict.Should().NotBeNull();
            conflict!.StatusCode.Should().Be(409);
            var value = conflict.Value as ErrorResponse;
            value!.Message.Should().Be("ChassisId já cadastrado.");
            //conflict.Value.Should().Be("ChassisId já cadastrado.");
        }
        
        [Fact]
        public async Task UpdateTruck_ShouldReturnOk_WhenTruckIsUpdated()
        {
            // Arrange
            var truck = new Truck { Id = 1, Model = TruckModel.VM, ManufactureYear = 2019, ChassisId = "CH123", Color = "Gray", Plant = Plant.Suécia };

            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(truck);
            _mockService.Setup(s => s.UpdateAsync(It.IsAny<Truck>())).ReturnsAsync(truck);

            // Act
            var result = await _controller.UpdateTruck(1, truck);
            var okResult = result as OkObjectResult;
            var value = okResult!.Value as TruckDto;

            // Assert
            okResult.Should().NotBeNull();
            value!.Id.Should().Be(1);
            value.Color.Should().Be("Gray");
        }

        [Fact]
        public async Task UpdateTruck_ShouldReturnBadRequest_WhenIdMismatch()
        {
            // Arrange
            var truck = new Truck { Id = 2, Model = TruckModel.VM, ManufactureYear = 2019, ChassisId = "CH999", Color = "Black", Plant = Plant.Brasil };

            // Act
            var result = await _controller.UpdateTruck(1, truck);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task UpdateTruck_ShouldReturnNotFound_WhenTruckDoesNotExist()
        {
            // Arrange
            var truck = new Truck { Id = 1, Model = TruckModel.FH, ManufactureYear = 2019, ChassisId = "CH404", Color = "Silver", Plant = Plant.Brasil };

            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((Truck?)null);

            // Act
            var result = await _controller.UpdateTruck(1, truck);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenSuccess()
        {
            _mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

            var result = await _controller.Delete(1);

            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenFails()
        {
            _mockService.Setup(s => s.DeleteAsync(99)).ReturnsAsync(false);

            var result = await _controller.Delete(99);

            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
