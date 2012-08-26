using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace Orchard.WidgetStyler
{
    public class CustomStyleMigrations : DataMigrationImpl
    {

        public int Create()
        {

            SchemaBuilder.CreateTable("CustomStyleRecord", table => table
                                                                         .ContentPartRecord()
                                                                         .Column("CssStyle", DbType.String, col => col.WithLength(1000))


                );


            ContentDefinitionManager.AlterPartDefinition("CustomStyle", builder => builder.Attachable());
            return 1;
        }

        public int UpdateFrom1() {
            //by default attach to standard HtmlWidget
            ContentDefinitionManager.AlterTypeDefinition("HtmlWidget", builder =>
                                                                       builder.WithPart("CustomStyle"));

                
            return 2;
        }

    }
}