using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OVRDP;

// :)
public static class Theme
{
    private static bool IsVGUI = false;

    public static Vector4 HelpMarkerHover { get; private set; }
    public static Vector4 HelpMarkerActive { get; private set; }

    public static void Toggle()
    {
        if ( IsVGUI )
            SetDefault();
        else
            SetVGUI();
    }


    public static void SetDefault()
    {
        IsVGUI = false;

        // Dark style from ImThemes
        var style = ImGuiNET.ImGui.GetStyle();

        style.Alpha = 1.0f;
        style.DisabledAlpha = 0.6000000238418579f;
        style.WindowPadding = new Vector2( 8.0f, 8.0f );
        style.WindowRounding = 0.0f;
        style.WindowBorderSize = 1.0f;
        style.WindowMinSize = new Vector2( 32.0f, 32.0f );
        style.WindowTitleAlign = new Vector2( 0.0f, 0.5f );
        style.WindowMenuButtonPosition = ImGuiDir.Left;
        style.ChildRounding = 0.0f;
        style.ChildBorderSize = 1.0f;
        style.PopupRounding = 0.0f;
        style.PopupBorderSize = 1.0f;
        style.FramePadding = new Vector2( 4.0f, 3.0f );
        style.FrameRounding = 0.0f;
        style.FrameBorderSize = 0.0f;
        style.ItemSpacing = new Vector2( 8.0f, 4.0f );
        style.ItemInnerSpacing = new Vector2( 4.0f, 4.0f );
        style.CellPadding = new Vector2( 4.0f, 2.0f );
        style.IndentSpacing = 21.0f;
        style.ColumnsMinSpacing = 6.0f;
        style.ScrollbarSize = 14.0f;
        style.ScrollbarRounding = 9.0f;
        style.GrabMinSize = 10.0f;
        style.GrabRounding = 0.0f;
        style.TabRounding = 4.0f;
        style.TabBorderSize = 0.0f;
        style.TabMinWidthForCloseButton = 0.0f;
        style.ColorButtonPosition = ImGuiDir.Right;
        style.ButtonTextAlign = new Vector2( 0.5f, 0.5f );
        style.SelectableTextAlign = new Vector2( 0.0f, 0.0f );

        style.Colors[(int) ImGuiCol.Text] = new Vector4( 1.0f, 1.0f, 1.0f, 1.0f );
        style.Colors[(int) ImGuiCol.TextDisabled] = new Vector4( 0.4980392158031464f, 0.4980392158031464f, 0.4980392158031464f, 1.0f );
        style.Colors[(int) ImGuiCol.WindowBg] = new Vector4( 0.05882352963089943f, 0.05882352963089943f, 0.05882352963089943f, 0.9399999976158142f );
        style.Colors[(int) ImGuiCol.ChildBg] = new Vector4( 0.0f, 0.0f, 0.0f, 1.0f );
        style.Colors[(int) ImGuiCol.PopupBg] = new Vector4( 0.0784313753247261f, 0.0784313753247261f, 0.0784313753247261f, 0.9399999976158142f );
        style.Colors[(int) ImGuiCol.Border] = new Vector4( 0.4274509847164154f, 0.4274509847164154f, 0.4980392158031464f, 0.5f );
        style.Colors[(int) ImGuiCol.BorderShadow] = new Vector4( 0.0f, 0.0f, 0.0f, 0.0f );
        style.Colors[(int) ImGuiCol.FrameBg] = new Vector4( 0.1568627506494522f, 0.2862745225429535f, 0.47843137383461f, 0.5400000214576721f );
        style.Colors[(int) ImGuiCol.FrameBgHovered] = new Vector4( 0.2588235437870026f, 0.5882353186607361f, 0.9764705896377563f, 0.4000000059604645f );
        style.Colors[(int) ImGuiCol.FrameBgActive] = new Vector4( 0.2588235437870026f, 0.5882353186607361f, 0.9764705896377563f, 0.6700000166893005f );
        style.Colors[(int) ImGuiCol.TitleBg] = new Vector4( 0.1568627506494522f, 0.2862745225429535f, 0.47843137383461f, 1.0f );
        style.Colors[(int) ImGuiCol.TitleBgActive] = new Vector4( 0.1568627506494522f, 0.2862745225429535f, 0.47843137383461f, 1.0f );
        style.Colors[(int) ImGuiCol.TitleBgCollapsed] = new Vector4( 0.0f, 0.0f, 0.0f, 0.5099999904632568f );
        style.Colors[(int) ImGuiCol.MenuBarBg] = new Vector4( 0.1372549086809158f, 0.1372549086809158f, 0.1372549086809158f, 1.0f );
        style.Colors[(int) ImGuiCol.ScrollbarBg] = new Vector4( 0.01960784383118153f, 0.01960784383118153f, 0.01960784383118153f, 0.5299999713897705f );
        style.Colors[(int) ImGuiCol.ScrollbarGrab] = new Vector4( 0.3098039329051971f, 0.3098039329051971f, 0.3098039329051971f, 1.0f );
        style.Colors[(int) ImGuiCol.ScrollbarGrabHovered] = new Vector4( 0.407843142747879f, 0.407843142747879f, 0.407843142747879f, 1.0f );
        style.Colors[(int) ImGuiCol.ScrollbarGrabActive] = new Vector4( 0.5098039507865906f, 0.5098039507865906f, 0.5098039507865906f, 1.0f );
        style.Colors[(int) ImGuiCol.CheckMark] = new Vector4( 0.2599999904632568f, 0.5899999737739563f, 0.9800000190734863f, 1.0f );
        style.Colors[(int) ImGuiCol.SliderGrab] = new Vector4( 0.239999994635582f, 0.5199999809265137f, 0.8799999952316284f, 1.0f );
        style.Colors[(int) ImGuiCol.SliderGrabActive] = new Vector4( 0.2599999904632568f, 0.5899999737739563f, 0.9800000190734863f, 1.0f );
        style.Colors[(int) ImGuiCol.Button] = new Vector4( 0.2588235437870026f, 0.5882353186607361f, 0.9764705896377563f, 0.4000000059604645f );
        style.Colors[(int) ImGuiCol.ButtonHovered] = new Vector4( 0.2588235437870026f, 0.5882353186607361f, 0.9764705896377563f, 1.0f );
        style.Colors[(int) ImGuiCol.ButtonActive] = new Vector4( 0.05882352963089943f, 0.529411792755127f, 0.9764705896377563f, 1.0f );
        HelpMarkerHover = new Vector4( 0.25f, 0.25f, 0.25f, 0.8f );
        HelpMarkerActive = new Vector4( 0.20f, 0.20f, 0.20f, 0.8f );
        style.Colors[(int) ImGuiCol.Header] = new Vector4( 0.2588235437870026f, 0.5882353186607361f, 0.9764705896377563f, 0.3100000023841858f );
        style.Colors[(int) ImGuiCol.HeaderHovered] = new Vector4( 0.2588235437870026f, 0.5882353186607361f, 0.9764705896377563f, 0.800000011920929f );
        style.Colors[(int) ImGuiCol.HeaderActive] = new Vector4( 0.2588235437870026f, 0.5882353186607361f, 0.9764705896377563f, 1.0f );
        style.Colors[(int) ImGuiCol.Separator] = new Vector4( 0.4274509847164154f, 0.4274509847164154f, 0.4980392158031464f, 0.5f );
        style.Colors[(int) ImGuiCol.SeparatorHovered] = new Vector4( 0.09803921729326248f, 0.4000000059604645f, 0.7490196228027344f, 0.7799999713897705f );
        style.Colors[(int) ImGuiCol.SeparatorActive] = new Vector4( 0.09803921729326248f, 0.4000000059604645f, 0.7490196228027344f, 1.0f );
        style.Colors[(int) ImGuiCol.ResizeGrip] = new Vector4( 0.2588235437870026f, 0.5882353186607361f, 0.9764705896377563f, 0.2000000029802322f );
        style.Colors[(int) ImGuiCol.ResizeGripHovered] = new Vector4( 0.2588235437870026f, 0.5882353186607361f, 0.9764705896377563f, 0.6700000166893005f );
        style.Colors[(int) ImGuiCol.ResizeGripActive] = new Vector4( 0.2588235437870026f, 0.5882353186607361f, 0.9764705896377563f, 0.949999988079071f );
        style.Colors[(int) ImGuiCol.Tab] = new Vector4( 0.1764705926179886f, 0.3490196168422699f, 0.5764706134796143f, 0.8619999885559082f );
        style.Colors[(int) ImGuiCol.TabHovered] = new Vector4( 0.2588235437870026f, 0.5882353186607361f, 0.9764705896377563f, 0.800000011920929f );
        style.Colors[(int) ImGuiCol.TabActive] = new Vector4( 0.196078434586525f, 0.407843142747879f, 0.6784313917160034f, 1.0f );
        style.Colors[(int) ImGuiCol.TabUnfocused] = new Vector4( 0.06666667014360428f, 0.1019607856869698f, 0.1450980454683304f, 0.9724000096321106f );
        style.Colors[(int) ImGuiCol.TabUnfocusedActive] = new Vector4( 0.1333333402872086f, 0.2588235437870026f, 0.4235294163227081f, 1.0f );
        style.Colors[(int) ImGuiCol.PlotLines] = new Vector4( 0.6078431606292725f, 0.6078431606292725f, 0.6078431606292725f, 1.0f );
        style.Colors[(int) ImGuiCol.PlotLinesHovered] = new Vector4( 1.0f, 0.4274509847164154f, 0.3490196168422699f, 1.0f );
        style.Colors[(int) ImGuiCol.PlotHistogram] = new Vector4( 0.8980392217636108f, 0.6980392336845398f, 0.0f, 1.0f );
        style.Colors[(int) ImGuiCol.PlotHistogramHovered] = new Vector4( 1.0f, 0.6000000238418579f, 0.0f, 1.0f );
        style.Colors[(int) ImGuiCol.TableHeaderBg] = new Vector4( 0.1882352977991104f, 0.1882352977991104f, 0.2000000029802322f, 1.0f );
        style.Colors[(int) ImGuiCol.TableBorderStrong] = new Vector4( 0.3098039329051971f, 0.3098039329051971f, 0.3490196168422699f, 1.0f );
        style.Colors[(int) ImGuiCol.TableBorderLight] = new Vector4( 0.2274509817361832f, 0.2274509817361832f, 0.2470588237047195f, 1.0f );
        style.Colors[(int) ImGuiCol.TableRowBg] = new Vector4( 0.0f, 0.0f, 0.0f, 0.0f );
        style.Colors[(int) ImGuiCol.TableRowBgAlt] = new Vector4( 1.0f, 1.0f, 1.0f, 0.05999999865889549f );
        style.Colors[(int) ImGuiCol.TextSelectedBg] = new Vector4( 0.2588235437870026f, 0.5882353186607361f, 0.9764705896377563f, 0.3499999940395355f );
        style.Colors[(int) ImGuiCol.DragDropTarget] = new Vector4( 1.0f, 1.0f, 0.0f, 0.8999999761581421f );
        style.Colors[(int) ImGuiCol.NavHighlight] = new Vector4( 0.2588235437870026f, 0.5882353186607361f, 0.9764705896377563f, 1.0f );
        style.Colors[(int) ImGuiCol.NavWindowingHighlight] = new Vector4( 1.0f, 1.0f, 1.0f, 0.699999988079071f );
        style.Colors[(int) ImGuiCol.NavWindowingDimBg] = new Vector4( 0.800000011920929f, 0.800000011920929f, 0.800000011920929f, 0.2000000029802322f );
        style.Colors[(int) ImGuiCol.ModalWindowDimBg] = new Vector4( 0.800000011920929f, 0.800000011920929f, 0.800000011920929f, 0.3499999940395355f );
    }

    public static void SetVGUI()
    {
        IsVGUI = true;

        // Fork of Classic Steam style from ImThemes
        var style = ImGuiNET.ImGui.GetStyle();

        style.Alpha = 1.0f;
        style.DisabledAlpha = 0.6000000238418579f;
        style.WindowPadding = new Vector2( 8.0f, 8.0f );
        style.WindowRounding = 0.0f;
        style.WindowBorderSize = 1.0f;
        style.WindowMinSize = new Vector2( 32.0f, 32.0f );
        style.WindowTitleAlign = new Vector2( 0.0f, 0.5f );
        style.WindowMenuButtonPosition = ImGuiDir.Left;
        style.ChildRounding = 0.0f;
        style.ChildBorderSize = 1.0f;
        style.PopupRounding = 0.0f;
        style.PopupBorderSize = 1.0f;
        style.FramePadding = new Vector2( 4.0f, 3.0f );
        style.FrameRounding = 0.0f;
        style.FrameBorderSize = 1.0f;
        style.ItemSpacing = new Vector2( 8.0f, 4.0f );
        style.ItemInnerSpacing = new Vector2( 4.0f, 4.0f );
        style.CellPadding = new Vector2( 4.0f, 2.0f );
        style.IndentSpacing = 21.0f;
        style.ColumnsMinSpacing = 6.0f;
        style.ScrollbarSize = 14.0f;
        style.ScrollbarRounding = 0.0f;
        style.GrabMinSize = 10.0f;
        style.GrabRounding = 0.0f;
        style.TabRounding = 0.0f;
        style.TabBorderSize = 0.0f;
        style.TabMinWidthForCloseButton = 0.0f;
        style.ColorButtonPosition = ImGuiDir.Right;
        style.ButtonTextAlign = new Vector2( 0.5f, 0.5f );
        style.SelectableTextAlign = new Vector2( 0.0f, 0.0f );

        style.Colors[(int) ImGuiCol.Text] = new Vector4( 1.0f, 1.0f, 1.0f, 1.0f );
        style.Colors[(int) ImGuiCol.TextDisabled] = new Vector4( 0.4980392158031464f, 0.4980392158031464f, 0.4980392158031464f, 1.0f );
        style.Colors[(int) ImGuiCol.WindowBg] = new Vector4( 0.2862745225429535f, 0.3372549116611481f, 0.2588235437870026f, 1.0f );
        style.Colors[(int) ImGuiCol.ChildBg] = new Vector4( 0.2167401760816574f, 0.2549019753932953f, 0.1959246546030045f, 1.0f );
        style.Colors[(int) ImGuiCol.PopupBg] = new Vector4( 0.239215686917305f, 0.2666666805744171f, 0.2000000029802322f, 1.0f );
        style.Colors[(int) ImGuiCol.Border] = new Vector4( 0.5372549295425415f, 0.5686274766921997f, 0.5098039507865906f, 0.5f );
        style.Colors[(int) ImGuiCol.BorderShadow] = new Vector4( 0.1372549086809158f, 0.1568627506494522f, 0.1098039224743843f, 0.5199999809265137f );
        style.Colors[(int) ImGuiCol.FrameBg] = new Vector4( 0.239215686917305f, 0.2666666805744171f, 0.2000000029802322f, 1.0f );
        style.Colors[(int) ImGuiCol.FrameBgHovered] = new Vector4( 0.2666666805744171f, 0.2980392277240753f, 0.2274509817361832f, 1.0f );
        style.Colors[(int) ImGuiCol.FrameBgActive] = new Vector4( 0.2980392277240753f, 0.3372549116611481f, 0.2588235437870026f, 1.0f );
        style.Colors[(int) ImGuiCol.TitleBg] = new Vector4( 0.239215686917305f, 0.2666666805744171f, 0.2000000029802322f, 1.0f );
        style.Colors[(int) ImGuiCol.TitleBgActive] = new Vector4( 0.239215686917305f, 0.2666666805744171f, 0.2000000029802322f, 1.0f );
        style.Colors[(int) ImGuiCol.TitleBgCollapsed] = new Vector4( 0.0f, 0.0f, 0.0f, 0.5099999904632568f );
        style.Colors[(int) ImGuiCol.MenuBarBg] = new Vector4( 0.239215686917305f, 0.2666666805744171f, 0.2000000029802322f, 1.0f );
        style.Colors[(int) ImGuiCol.ScrollbarBg] = new Vector4( 0.3490196168422699f, 0.4196078479290009f, 0.3098039329051971f, 1.0f );
        style.Colors[(int) ImGuiCol.ScrollbarGrab] = new Vector4( 0.2784313857555389f, 0.3176470696926117f, 0.239215686917305f, 1.0f );
        style.Colors[(int) ImGuiCol.ScrollbarGrabHovered] = new Vector4( 0.2470588237047195f, 0.2980392277240753f, 0.2196078449487686f, 1.0f );
        style.Colors[(int) ImGuiCol.ScrollbarGrabActive] = new Vector4( 0.2274509817361832f, 0.2666666805744171f, 0.2078431397676468f, 1.0f );
        style.Colors[(int) ImGuiCol.CheckMark] = new Vector4( 0.5882353186607361f, 0.5372549295425415f, 0.1764705926179886f, 1.0f );
        style.Colors[(int) ImGuiCol.SliderGrab] = new Vector4( 0.3490196168422699f, 0.4196078479290009f, 0.3098039329051971f, 1.0f );
        style.Colors[(int) ImGuiCol.SliderGrabActive] = new Vector4( 0.5372549295425415f, 0.5686274766921997f, 0.5098039507865906f, 0.5f );
        style.Colors[(int) ImGuiCol.Button] = new Vector4( 0.3834633529186249f, 0.4509803950786591f, 0.3466359078884125f, 0.4000000059604645f );
        style.Colors[(int) ImGuiCol.ButtonHovered] = new Vector4( 0.3490196168422699f, 0.4196078479290009f, 0.3098039329051971f, 1.0f );
        style.Colors[(int) ImGuiCol.ButtonActive] = new Vector4( 0.5372549295425415f, 0.5686274766921997f, 0.5098039507865906f, 0.5f );
        HelpMarkerHover = new Vector4( 0.3490196168422699f, 0.4196078479290009f, 0.3098039329051971f, 1.0f );
        HelpMarkerActive = new Vector4( 0.5372549295425415f, 0.5686274766921997f, 0.5098039507865906f, 0.5f );
        style.Colors[(int) ImGuiCol.Header] = new Vector4( 0.3490196168422699f, 0.4196078479290009f, 0.3098039329051971f, 1.0f );
        style.Colors[(int) ImGuiCol.HeaderHovered] = new Vector4( 0.3490196168422699f, 0.4196078479290009f, 0.3098039329051971f, 0.6000000238418579f );
        style.Colors[(int) ImGuiCol.HeaderActive] = new Vector4( 0.5372549295425415f, 0.5686274766921997f, 0.5098039507865906f, 0.5f );
        style.Colors[(int) ImGuiCol.Separator] = new Vector4( 0.1372549086809158f, 0.1568627506494522f, 0.1098039224743843f, 1.0f );
        style.Colors[(int) ImGuiCol.SeparatorHovered] = new Vector4( 0.5372549295425415f, 0.5686274766921997f, 0.5098039507865906f, 1.0f );
        style.Colors[(int) ImGuiCol.SeparatorActive] = new Vector4( 0.5882353186607361f, 0.5372549295425415f, 0.1764705926179886f, 1.0f );
        style.Colors[(int) ImGuiCol.ResizeGrip] = new Vector4( 0.1882352977991104f, 0.2274509817361832f, 0.1764705926179886f, 0.0f );
        style.Colors[(int) ImGuiCol.ResizeGripHovered] = new Vector4( 0.5372549295425415f, 0.5686274766921997f, 0.5098039507865906f, 1.0f );
        style.Colors[(int) ImGuiCol.ResizeGripActive] = new Vector4( 0.5882353186607361f, 0.5372549295425415f, 0.1764705926179886f, 1.0f );
        style.Colors[(int) ImGuiCol.Tab] = new Vector4( 0.3490196168422699f, 0.4196078479290009f, 0.3098039329051971f, 1.0f );
        style.Colors[(int) ImGuiCol.TabHovered] = new Vector4( 0.5372549295425415f, 0.5686274766921997f, 0.5098039507865906f, 0.7799999713897705f );
        style.Colors[(int) ImGuiCol.TabActive] = new Vector4( 0.5882353186607361f, 0.5372549295425415f, 0.1764705926179886f, 1.0f );
        style.Colors[(int) ImGuiCol.TabUnfocused] = new Vector4( 0.239215686917305f, 0.2666666805744171f, 0.2000000029802322f, 1.0f );
        style.Colors[(int) ImGuiCol.TabUnfocusedActive] = new Vector4( 0.3490196168422699f, 0.4196078479290009f, 0.3098039329051971f, 1.0f );
        style.Colors[(int) ImGuiCol.PlotLines] = new Vector4( 0.6078431606292725f, 0.6078431606292725f, 0.6078431606292725f, 1.0f );
        style.Colors[(int) ImGuiCol.PlotLinesHovered] = new Vector4( 0.5882353186607361f, 0.5372549295425415f, 0.1764705926179886f, 1.0f );
        style.Colors[(int) ImGuiCol.PlotHistogram] = new Vector4( 1.0f, 0.7764706015586853f, 0.2784313857555389f, 1.0f );
        style.Colors[(int) ImGuiCol.PlotHistogramHovered] = new Vector4( 1.0f, 0.6000000238418579f, 0.0f, 1.0f );
        style.Colors[(int) ImGuiCol.TableHeaderBg] = new Vector4( 0.1882352977991104f, 0.1882352977991104f, 0.2000000029802322f, 1.0f );
        style.Colors[(int) ImGuiCol.TableBorderStrong] = new Vector4( 0.3098039329051971f, 0.3098039329051971f, 0.3490196168422699f, 1.0f );
        style.Colors[(int) ImGuiCol.TableBorderLight] = new Vector4( 0.2274509817361832f, 0.2274509817361832f, 0.2470588237047195f, 1.0f );
        style.Colors[(int) ImGuiCol.TableRowBg] = new Vector4( 0.0f, 0.0f, 0.0f, 0.0f );
        style.Colors[(int) ImGuiCol.TableRowBgAlt] = new Vector4( 1.0f, 1.0f, 1.0f, 0.05999999865889549f );
        style.Colors[(int) ImGuiCol.TextSelectedBg] = new Vector4( 0.5882353186607361f, 0.5372549295425415f, 0.1764705926179886f, 1.0f );
        style.Colors[(int) ImGuiCol.DragDropTarget] = new Vector4( 0.729411780834198f, 0.6666666865348816f, 0.239215686917305f, 1.0f );
        style.Colors[(int) ImGuiCol.NavHighlight] = new Vector4( 0.5882353186607361f, 0.5372549295425415f, 0.1764705926179886f, 1.0f );
        style.Colors[(int) ImGuiCol.NavWindowingHighlight] = new Vector4( 1.0f, 1.0f, 1.0f, 0.699999988079071f );
        style.Colors[(int) ImGuiCol.NavWindowingDimBg] = new Vector4( 0.800000011920929f, 0.800000011920929f, 0.800000011920929f, 0.2000000029802322f );
        style.Colors[(int) ImGuiCol.ModalWindowDimBg] = new Vector4( 0.800000011920929f, 0.800000011920929f, 0.800000011920929f, 0.3499999940395355f );
    }
}
