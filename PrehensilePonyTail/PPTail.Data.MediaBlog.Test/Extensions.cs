﻿using Moq;
using PPTail.Entities;
using PPTail.Enumerations;
using PPTail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestHelperExtensions;

namespace PPTail.Data.MediaBlog.Test
{
    public static class Extensions
    {
        //const string _sourceDataPathSettingName = "sourceDataPath";
        //const string _connectionStringFilepathKey = "FilePath";

        //const string _widgetZoneNodeFormat = "<widget id=\"{0}\" title=\"{1}\" showTitle=\"{2}\">{3}</widget>";
        //const string _categoryNodeFormat = "<category id=\"{0}\" description=\"{2}\" parent=\"\">{1}</category>";

        public static string GetRandomUrl(this string ignore)
        {
            return $"http://www.{string.Empty.GetRandom()}.com/{string.Empty.GetRandom()}";
        }

        //public static XElement ConditionalAddNode(this XElement node, bool addNode, string name, string value)
        //{
        //    if (addNode)
        //        node.Add(new XElement(XName.Get(name), value));
        //    return node;
        //}

        //public static XElement Serialize(this IEnumerable<Widget> widgets)
        //{
        //    var widgetNode = XElement.Parse("<?xml version=\"1.0\" encoding=\"utf-8\"?><widgets></widgets>");
        //    foreach (var widget in widgets)
        //    {
        //        string nodeText = string.Format(_widgetZoneNodeFormat, widget.Id.ToString(), widget.Title, widget.ShowTitle.ToString(), widget.WidgetType.Serialize());
        //        widgetNode.Add(XElement.Parse(nodeText));
        //    }

        //    return widgetNode;
        //}

        public static string Serialize(this WidgetType widgetType)
        {
            return widgetType.ToString().Replace("_", " ");
        }

        //public static XElement Serialize(this IEnumerable<Category> categories)
        //{
        //    throw new NotImplementedException();

        //    var rootNode = XElement.Parse("<?xml version=\"1.0\" encoding=\"utf-8\"?><categories></categories>");
        //    foreach (var category in categories)
        //    {
        //        string nodeText = string.Format(_categoryNodeFormat, category.Id.ToString(), category.Name, category.Description);
        //        rootNode.Add(XElement.Parse(nodeText));
        //    }

        //    return rootNode;
        //}

    }
}