using GameStop;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Tourfirm.DAL;
using Tourfirm.DAL.Interfaces;
using Tourfirm.Domain.Entity;
using Tourfirm.Domain.Response;
using Tourfirm.Domain.Safety;
using Tourfirm.Domain.ViewModels;
using Tourfirm.Service.Interfaces;

namespace Tourfirm.Service.Implementations;

public class TourService : ITourService
{
    private readonly ILogger<TourService> _logger;
    private readonly ApplicationContext _db;
    private readonly ITour _tourRepository;
    private readonly ITourImage _tourImageRepository;
    private readonly IWebHostEnvironment _app;

    public TourService(ILogger<TourService> logger, ApplicationContext db, ITour tourRepository, IWebHostEnvironment app, ITourImage tourImageRepository)
    {
        _logger = logger;
        _db = db;
        _tourRepository = tourRepository;
        _app = app;
        _tourImageRepository = tourImageRepository;
    }

    public async Task<BaseResponse<bool>> CreateTour(Tour tour,  TourAddViewModel tourAddViewModel)
    {
        try
        {
            await _tourRepository.addTour(tour); 
        
            if (tourAddViewModel.Files.Count > 0)
            {
                foreach (var file in tourAddViewModel.Files)
                {
                    if (FormFileExtensions.IsImage(file))
                    {
                        string path = "/images/" + file.FileName;
                        using (FileStream fileStream = new FileStream(_app.WebRootPath + path,
                                   FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        await _tourImageRepository.addTourImage(new TourImage()
                            { Path = $"~/images/{file.FileName}", TourId = tour.Id });
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        return new BaseResponse<bool>()
                        {
                            StatusCode = StatusCode.NoFormat,
                            Description = "There is not correct format"
                        };
                    }
                }
            }
            else
            {
                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.NoImages,
                    Description = "There are no images for this tour"
                };
            }

            return new BaseResponse<bool>()
            {
                Data = true,
                StatusCode = StatusCode.OK,
                Description = "Tour creation procedure was successfully completed"
            };
        }

        catch(Exception ex)
        {
            _logger.LogError(ex, $"[Creating Tour Procedure]: {ex.Message}");
            return new BaseResponse<bool>()
            {
                Description = ex.Message,
                StatusCode = StatusCode.InternalServerError
            };
        }
    }

    public async Task<BaseResponse<bool>> UpdateTour(Tour tourModel, TourUpdateViewModel tourViewModel)
    {
        try
        {
            var tour = await _tourRepository.getTour(tourViewModel.Id);

            if (tour == null)
            {
                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.TourNotFound,
                    Description = "Tour not found"
                };
            }
            
            if (tourViewModel.Files != null)
            {
                foreach (var file in tourViewModel.Files)
                {
                    if (FormFileExtensions.IsImage(file))
                    {
                        string path = "/images/" + file.FileName;
                        using (FileStream fileStream = new FileStream(_app.WebRootPath + path,
                                   FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        await _tourImageRepository.addTourImage(new TourImage()
                            { Path = $"~/images/{file.FileName}", TourId = tour.Id });
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        return new BaseResponse<bool>()
                        {
                            StatusCode = StatusCode.NoFormat,
                            Description = "There is not correct format"
                        };
                    }
                }
            }
            

            tour.Cost = tourModel.Cost;
            tour.CountryId = tourModel.CountryId;
            tour.Description = tourViewModel.Description;
            tour.HotelId = tourModel.HotelId;
            tour.Name = tourViewModel.Name;
            tour.RouteId = tourModel.RouteId;
            tour.TourImages = tourModel.TourImages;
            tour.TourTypeId = tourModel.TourTypeId;

            _tourRepository.updateTour(tour);

            return new BaseResponse<bool>()
            {
                Data = true,
                StatusCode = StatusCode.OK,
                Description = "Tour was updated"
            };
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, $"[UpdateTour]: {ex.Message}");
            return new BaseResponse<bool>()
            {
                Description = ex.Message,
                StatusCode = StatusCode.InternalServerError
            };
        } 
    }

    public async Task<BaseResponse<bool>> DeleteTour(Tour tour)
    {
        try
        {
             _tourRepository.deleteTour(tour.Id);
             
             return new BaseResponse<bool>()
             {
                 Data = true,
                 StatusCode = StatusCode.OK,
                 Description = "Tour delete procedure was successfully completed"
             };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[Creating Tour Procedure]: {ex.Message}");
            return new BaseResponse<bool>()
            {
                Description = ex.Message,
                StatusCode = StatusCode.InternalServerError
            };
        }
    }
}