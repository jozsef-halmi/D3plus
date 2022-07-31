using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Types;

namespace Infrastructure.GraphQL.Types;

public class CategoryType : ObjectGraphType<Category>
{
    public CategoryType(CategoriesData data)
    {
        Name = "Category";
        Description = "A category of a product";

        Field(d => d.Id, nullable: true).Description("The id of the categorz.");
        Field(d => d.Name, nullable: true).Description("The description of the category.");

        //Field<ListGraphType<CharacterInterface>>(
        //    "friends",
        //    resolve: context => data.GetFriends(context.Source)
        //);
        //Field<ListGraphType<EpisodeEnum>>("appearsIn", "Which movie they appear in.");
        //Field(d => d.PrimaryFunction, nullable: true).Description("The primary function of the droid.");

        //Interface<CharacterInterface>();
    }
}

