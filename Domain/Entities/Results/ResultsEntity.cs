using System.ComponentModel.DataAnnotations;

namespace BichoApi.Domain.Entities.Results;

public class ResultsEntity
{
    [Key] public int Id { get; init; }
}