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
                            StatusCode = StatusCode.NoImages,
                            Description = "There are no images for tour"
                        };
                    }
                }
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
}