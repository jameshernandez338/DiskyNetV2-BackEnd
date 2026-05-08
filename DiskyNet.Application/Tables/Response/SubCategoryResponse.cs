namespace DiskyNet.Application.Tables.Response
{
    public record SubCategoryResponse(
        int SubCategoryId,
        string SubCategoryName,
        int CategoryId,
        string CategoryName
    );
}
