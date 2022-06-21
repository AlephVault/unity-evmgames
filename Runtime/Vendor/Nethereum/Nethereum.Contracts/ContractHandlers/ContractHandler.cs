﻿using System.Threading;
using System.Threading.Tasks;
using AlephVault.Unity.EVMGames.Nethereum.ABI.Decoders;
using AlephVault.Unity.EVMGames.Nethereum.ABI.FunctionEncoding.Attributes;
using AlephVault.Unity.EVMGames.Nethereum.Contracts.CQS;
using AlephVault.Unity.EVMGames.Nethereum.Contracts.Services;
using AlephVault.Unity.EVMGames.Nethereum.Hex.HexTypes;
using AlephVault.Unity.EVMGames.Nethereum.RPC.Eth.DTOs;

namespace AlephVault.Unity.EVMGames.Nethereum.Contracts.ContractHandlers
{
    public class ContractHandler
    {
        public ContractHandler(string contractAddress, EthApiContractService ethApiContractService,
            string addressFrom = null)
        {
            ContractAddress = contractAddress;
            EthApiContractService = ethApiContractService;
            AddressFrom = addressFrom;
        }

        protected string AddressFrom { get; set; }

        public string ContractAddress { get; }
        public EthApiContractService EthApiContractService { get; }

        public Event<TEventType> GetEvent<TEventType>() where TEventType : IEventDTO, new()
        {
            if (!EventAttribute.IsEventType(typeof(TEventType))) return null;
            return new Event<TEventType>(EthApiContractService.Client, ContractAddress);
        }

        public Function<TEthereumContractFunctionMessage> GetFunction<TEthereumContractFunctionMessage>() where TEthereumContractFunctionMessage : new()
        {
            var contract = EthApiContractService.GetContract<TEthereumContractFunctionMessage>(ContractAddress);
            return contract.GetFunction<TEthereumContractFunctionMessage>();
        }

        protected void SetAddressFrom(ContractMessageBase contractMessage)
        {
            contractMessage.FromAddress = contractMessage.FromAddress ?? AddressFrom;
        }

#if !DOTNET35

        public Task<TransactionReceipt> SendRequestAndWaitForReceiptAsync<TEthereumContractFunctionMessage>(
            TEthereumContractFunctionMessage transactionMessage = null, CancellationTokenSource tokenSource = null)
            where TEthereumContractFunctionMessage : FunctionMessage, new()
        {
            if (transactionMessage == null) transactionMessage = new TEthereumContractFunctionMessage();
            var command = EthApiContractService.GetContractTransactionHandler<TEthereumContractFunctionMessage>();
            SetAddressFrom(transactionMessage);
            return command.SendRequestAndWaitForReceiptAsync(ContractAddress, transactionMessage, tokenSource);
        }
  
        public Task<string> SendRequestAsync<TEthereumContractFunctionMessage>(
            TEthereumContractFunctionMessage transactionMessage = null)
            where TEthereumContractFunctionMessage : FunctionMessage, new()
        {
            if(transactionMessage == null) transactionMessage = new TEthereumContractFunctionMessage();
            var command = EthApiContractService.GetContractTransactionHandler<TEthereumContractFunctionMessage>();
            SetAddressFrom(transactionMessage);
            return command.SendRequestAsync(ContractAddress, transactionMessage);
        }

        public Task<string> SignTransactionAsync<TEthereumContractFunctionMessage>(
            TEthereumContractFunctionMessage transactionMessage = null)
            where TEthereumContractFunctionMessage : FunctionMessage, new()
        {
            if (transactionMessage == null) transactionMessage = new TEthereumContractFunctionMessage();
            var command = EthApiContractService.GetContractTransactionHandler<TEthereumContractFunctionMessage>();
            SetAddressFrom(transactionMessage);
            return command.SignTransactionAsync(ContractAddress, transactionMessage);
        }

        public Task<HexBigInteger> EstimateGasAsync<TEthereumContractFunctionMessage>(
            TEthereumContractFunctionMessage transactionMessage = null)
            where TEthereumContractFunctionMessage : FunctionMessage, new()
        {
            if (transactionMessage == null) transactionMessage = new TEthereumContractFunctionMessage();
            var command = EthApiContractService.GetContractTransactionHandler<TEthereumContractFunctionMessage>();
            SetAddressFrom(transactionMessage);
            return command.EstimateGasAsync(ContractAddress, transactionMessage);
        }

        public Task<TEthereumFunctionReturn> QueryDeserializingToObjectAsync<TEthereumContractFunctionMessage,
            TEthereumFunctionReturn>(TEthereumContractFunctionMessage ethereumContractFunctionMessage = null,
            BlockParameter blockParameter = null)
            where TEthereumContractFunctionMessage : FunctionMessage, new()
            where TEthereumFunctionReturn : IFunctionOutputDTO, new()
        {
            if (ethereumContractFunctionMessage == null) ethereumContractFunctionMessage = new TEthereumContractFunctionMessage();
            SetAddressFrom(ethereumContractFunctionMessage);
            var queryHandler = EthApiContractService.GetContractQueryHandler<TEthereumContractFunctionMessage>();
            return queryHandler.QueryDeserializingToObjectAsync<TEthereumFunctionReturn>(
                ethereumContractFunctionMessage,
                ContractAddress, blockParameter);
        }

        public Task<TReturn> QueryAsync<TEthereumContractFunctionMessage, TReturn>(
            TEthereumContractFunctionMessage ethereumContractFunctionMessage = null, BlockParameter blockParameter = null)
            where TEthereumContractFunctionMessage : FunctionMessage, new()
        {
            if(ethereumContractFunctionMessage == null) ethereumContractFunctionMessage = new TEthereumContractFunctionMessage();
            SetAddressFrom(ethereumContractFunctionMessage);
            var queryHandler = EthApiContractService.GetContractQueryHandler<TEthereumContractFunctionMessage>();
            return queryHandler.QueryAsync<TReturn>(
                ContractAddress, ethereumContractFunctionMessage, blockParameter);
        }

        public Task<byte[]> QueryRawAsync<TEthereumContractFunctionMessage>(TEthereumContractFunctionMessage ethereumContractFunctionMessage = null, BlockParameter blockParameter = null)
            where TEthereumContractFunctionMessage : FunctionMessage, new()
        {
            if (ethereumContractFunctionMessage == null) ethereumContractFunctionMessage = new TEthereumContractFunctionMessage();
            SetAddressFrom(ethereumContractFunctionMessage);
            var queryHandler = EthApiContractService.GetContractQueryHandler<TEthereumContractFunctionMessage>();
            return queryHandler.QueryRawAsBytesAsync(
                ContractAddress, ethereumContractFunctionMessage, blockParameter);
        }
        
        public Task<TReturn> QueryRawAsync<TEthereumContractFunctionMessage, TCustomDecoder, TReturn>(BlockParameter blockParameter = null)
           where TEthereumContractFunctionMessage : FunctionMessage, new()
           where TCustomDecoder : ICustomRawDecoder<TReturn>, new()
        {
            var ethereumContractFunctionMessage = new TEthereumContractFunctionMessage();
            return QueryRawAsync<TEthereumContractFunctionMessage, TCustomDecoder, TReturn>(ethereumContractFunctionMessage, blockParameter);
        }

        public async Task<TReturn> QueryRawAsync<TEthereumContractFunctionMessage, TCustomDecoder, TReturn>(TEthereumContractFunctionMessage ethereumContractFunctionMessage, BlockParameter blockParameter = null)
            where TEthereumContractFunctionMessage : FunctionMessage, new()
            where TCustomDecoder : ICustomRawDecoder<TReturn>, new()
        {
          
            var result = await QueryRawAsync<TEthereumContractFunctionMessage>(ethereumContractFunctionMessage,
                blockParameter).ConfigureAwait(false);
            var decoder = new TCustomDecoder();
            return decoder.Decode(result);
        }
#endif

    }

}