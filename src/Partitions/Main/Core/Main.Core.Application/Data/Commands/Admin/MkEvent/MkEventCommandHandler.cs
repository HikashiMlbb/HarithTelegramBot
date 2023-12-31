﻿using System.Globalization;
using System.Text.RegularExpressions;
using Main.Core.Domain.Exceptions.Events;
using Main.Core.Domain.Repositories;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Application.Shared;
using TelegramBot.Partitions.Shared.Commands;

namespace Main.Core.Application.Data.Commands.Admin.MkEvent;

[Command("mkevnt")]
public class MkEventCommandHandler : ITextCommandHandler<MkEventCommand>
{
    private readonly ITelegramBotClient _bot;
    private readonly ILogger _logger = Log.ForContext<MkEventCommandHandler>();
    private readonly IUnitOfWork _uow;

    public MkEventCommandHandler(IBotService bot, IUnitOfWork uow)
    {
        _uow = uow;
        _bot = bot.CurrentBot;
    }

    public async Task ExecuteAsync(Message message, CancellationToken cancellationToken)
    {
        var userId = message.From!.Id;
        var chatId = message.Chat.Id;
        var chatMember = await _bot.GetChatMemberAsync(chatId, userId, cancellationToken);

        if (chatMember.Status != ChatMemberStatus.Creator) return;

        var args = message.Text!.Split(
                ' ',
                2,
                StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Last();

        args = args.Replace(',', '.');

        var regex = @"""(.+)""\s(\d+(\.|,)?\d*)$";
        var match = Regex.Match(args, regex);

        if (!match.Success)
        {
            await _bot.SendTextMessageAsync(
                chatId,
                "Вы ввели данные события в неверном формате.\nВерный формат:\n/mkevnt \"New year\" 0.15",
                cancellationToken: cancellationToken);
            return;
        }

        try
        {
            var result = await _uow.Events.AddAsync(
                match.Groups[1].Value,
                chatId,
                float.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture),
                cancellationToken);

            await _uow.CompleteAsync(cancellationToken);

            var messageToSend = $"Вы успешно добавили событие \"{result.Name}\"";
            await _bot.SendTextMessageAsync(chatId, messageToSend, replyToMessageId: message.MessageId,
                cancellationToken: cancellationToken);
        }
        catch (EventAlreadyExistException)
        {
            await _bot.SendTextMessageAsync(
                chatId,
                "Это событие уже существует :)",
                replyToMessageId: message.MessageId,
                cancellationToken: cancellationToken);
        }
        catch (Exception e)
        {
            _logger.Error("AddEventCommand exception: {exception}", e.Message);
        }
    }
}