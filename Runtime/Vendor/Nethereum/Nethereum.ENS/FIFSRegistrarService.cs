using System.Threading;
using System.Threading.Tasks;
using AlephVault.Unity.EVMGames.Nethereum.Contracts.ContractHandlers;
using AlephVault.Unity.EVMGames.Nethereum.ENS.FIFSRegistrar.ContractDefinition;
using AlephVault.Unity.EVMGames.Nethereum.RPC.Eth.DTOs;

namespace AlephVault.Unity.EVMGames.Nethereum.ENS
{

    public partial class FIFSRegistrarService
    {
    
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, FIFSRegistrarDeployment fIFSRegistrarDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<FIFSRegistrarDeployment>().SendRequestAndWaitForReceiptAsync(fIFSRegistrarDeployment, cancellationTokenSource);
        }
        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, FIFSRegistrarDeployment fIFSRegistrarDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<FIFSRegistrarDeployment>().SendRequestAsync(fIFSRegistrarDeployment);
        }
        public static async Task<FIFSRegistrarService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, FIFSRegistrarDeployment fIFSRegistrarDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, fIFSRegistrarDeployment, cancellationTokenSource);
            return new FIFSRegistrarService(web3, receipt.ContractAddress);
        }
    
        protected Nethereum.Web3.Web3 Web3{ get; }
        
        public ContractHandler ContractHandler { get; }
        
        public FIFSRegistrarService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }
    
        public Task<string> RegisterRequestAsync(RegisterFunction registerFunction)
        {
             return ContractHandler.SendRequestAsync(registerFunction);
        }

        public Task<TransactionReceipt> RegisterRequestAndWaitForReceiptAsync(RegisterFunction registerFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(registerFunction, cancellationToken);
        }

        public Task<string> RegisterRequestAsync(byte[] subnode, string owner)
        {
            var registerFunction = new RegisterFunction();
                registerFunction.Subnode = subnode;
                registerFunction.Owner = owner;
            
             return ContractHandler.SendRequestAsync(registerFunction);
        }

        public Task<TransactionReceipt> RegisterRequestAndWaitForReceiptAsync(byte[] subnode, string owner, CancellationTokenSource cancellationToken = null)
        {
            var registerFunction = new RegisterFunction();
                registerFunction.Subnode = subnode;
                registerFunction.Owner = owner;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(registerFunction, cancellationToken);
        }
    }
}
