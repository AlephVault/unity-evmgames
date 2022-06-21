﻿using System.Threading.Tasks;
using AlephVault.Unity.EVMGames.Nethereum.ABI.FunctionEncoding.Attributes;
using AlephVault.Unity.EVMGames.Nethereum.Contracts.QueryHandlers;
using AlephVault.Unity.EVMGames.Nethereum.Hex.HexConvertors.Extensions;
using AlephVault.Unity.EVMGames.Nethereum.RPC.Eth.DTOs;

namespace AlephVault.Unity.EVMGames.Nethereum.Contracts.ContractHandlers
{
#if !DOTNET35

    public abstract class ContractQueryHandlerBase<TFunctionMessage> : IContractQueryHandler<TFunctionMessage>
        where TFunctionMessage : FunctionMessage, new()
    {
        public Task<TFunctionOutput> QueryDeserializingToObjectAsync<TFunctionOutput>(
        TFunctionMessage functionMessage,
        string contractAddress,
        BlockParameter block = null)
            where TFunctionOutput : IFunctionOutputDTO, new()
        {
            var queryHandler = GetQueryDTOHandler<TFunctionOutput>();
            return queryHandler.QueryAsync(contractAddress, functionMessage, block);
        }

        protected abstract QueryToDTOHandler<TFunctionMessage, TFunctionOutput> GetQueryDTOHandler<TFunctionOutput>() where TFunctionOutput : IFunctionOutputDTO, new();

        public Task<TFunctionOutput> QueryAsync<TFunctionOutput>(
            string contractAddress,
            TFunctionMessage functionMessage = null,
            BlockParameter block = null)
        {
            var queryHandler = GetQueryToSimpleTypeHandler<TFunctionOutput>();
            return queryHandler.QueryAsync(contractAddress, functionMessage, block);
        }

        protected abstract QueryToSimpleTypeHandler<TFunctionMessage, TFunctionOutput> GetQueryToSimpleTypeHandler<TFunctionOutput>();

        public async Task<byte[]> QueryRawAsBytesAsync(
            string contractAddress,
            TFunctionMessage functionMessage = null,
            BlockParameter block = null)
        {
            var rawResult = await QueryRawAsync(contractAddress, functionMessage, block).ConfigureAwait(false);
            return rawResult.HexToByteArray();
        }

        public Task<string> QueryRawAsync(
            string contractAddress,
            TFunctionMessage functionMessage = null,
            BlockParameter block = null)
        {
            QueryRawHandler<TFunctionMessage> queryHandler = GetQueryRawHandler();
            return queryHandler.QueryAsync(contractAddress, functionMessage, block);
        }

        protected abstract QueryRawHandler<TFunctionMessage> GetQueryRawHandler();
    }
#endif
}