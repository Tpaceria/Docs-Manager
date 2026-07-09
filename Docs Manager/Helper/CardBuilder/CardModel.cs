using Microsoft.Maui.Graphics;

namespace Docs_Manager.Helper;

public class CardModel
{
    /// <summary>
    /// Заголовок карточки
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Поля карточки (Country, Number, Issued, Expiry...)
    /// </summary>
    public List<CardField> Fields { get; set; } = new();

    /// <summary>
    /// Цвет заголовка (при необходимости)
    /// </summary>
    public Color TitleColor { get; set; } = Colors.White;

    /// <summary>
    /// Действие "Просмотр"
    /// </summary>
    public Action? ViewAction { get; set; }

    /// <summary>
    /// Действие "Редактировать"
    /// </summary>
    public Action? EditAction { get; set; }

    /// <summary>
    /// Действие "Удалить"
    /// </summary>
    public Action? DeleteAction { get; set; }
}

/// <summary>
/// Одно поле в строке информации
/// </summary>
public class CardField
{
    /// <summary>
    /// Подпись (No., Issued, Expiry...)
    /// </summary>
    public string Caption { get; set; } = string.Empty;

    /// <summary>
    /// Значение поля
    /// </summary>
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// Цвет текста
    /// </summary>
    public Color TextColor { get; set; } = Color.FromArgb("#8fb3d9");

    /// <summary>
    /// Жирный шрифт
    /// </summary>
    public bool Bold { get; set; } = false;
}