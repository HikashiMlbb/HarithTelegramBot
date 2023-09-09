﻿using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramBot.Application.Data.Interfaces;

public interface IHandler
{
    public UpdateType UpdateType { get; }
    public Task HandleAsync(Update update, CancellationToken cancellationToken);
}