using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Designer.Interfaces;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using VSLangProj80;

namespace TypeSql.VisualStudio
{
    [ComVisible(true)]
    [Guid("DED9B845-2507-4B2E-B850-F52101E19C47")]
    [CodeGeneratorRegistration(typeof(TypeSqlGenerator), "TypeSql", vsContextGuids.vsContextGuidVCSProject, GeneratesDesignTimeSource = true)]
    [ProvideObject(typeof(TypeSqlGenerator))]
    public class TypeSqlGenerator : IVsSingleFileGenerator, IObjectWithSite 
    {
#pragma warning disable 0414
        //The name of this generator (use for 'Custom Tool' property of project item)
        internal static string name = "TypeSqlGenerator";
#pragma warning restore 0414

        public TypeSqlGenerator()
        {
            EnvDTE.DTE dte = (EnvDTE.DTE)Package.GetGlobalService(typeof(EnvDTE.DTE));
            Array ary = (Array)dte.ActiveSolutionProjects;
            if (ary.Length > 0)
            {
                project = (EnvDTE.Project)ary.GetValue(0);

            }
        }

        public int DefaultExtension(out string pbstrDefaultExtension)
        {
            try
            {
                pbstrDefaultExtension = GetDefaultExtension();
                return VSConstants.S_OK;
            }
            catch (Exception e)
            {
                Trace.WriteLine("Getting default extension failed:");
                Trace.WriteLine(e.ToString());
                pbstrDefaultExtension = string.Empty;
                return VSConstants.E_FAIL;
            }
        }

        public int Generate(string wszInputFilePath, string bstrInputFileContents, string wszDefaultNamespace, IntPtr[] rgbOutputFileContents, out uint pcbOutput,
                            IVsGeneratorProgress pGenerateProgress)
        {
            if (bstrInputFileContents == null)
            {
                throw new ArgumentNullException(bstrInputFileContents);
            }


            codeFilePath = wszInputFilePath;
            codeFileNameSpace = wszDefaultNamespace;
            codeGeneratorProgress = pGenerateProgress;

            int documentFound = 0;
            uint itemId = 0;
            EnvDTE.ProjectItem item = null;
            Microsoft.VisualStudio.Shell.Interop.VSDOCUMENTPRIORITY[] pdwPriority = new Microsoft.VisualStudio.Shell.Interop.VSDOCUMENTPRIORITY[1];

            // obtain a reference to the current project as an IVsProject type
            Microsoft.VisualStudio.Shell.Interop.IVsProject VsProject = VsHelper.ToVsProject(project);
            // this locates, and returns a handle to our source file, as a ProjectItem
            VsProject.IsDocumentInProject(wszInputFilePath, out documentFound, pdwPriority, out itemId);

            // if our source file was not found in the project (which it should have been)
            if (documentFound == 0 || itemId == 0)
            {
                // Return E_FAIL to inform Visual Studio that the generator has failed (so that no file gets generated)
                pcbOutput = 0;
                return VSConstants.E_FAIL;
            }

            Microsoft.VisualStudio.OLE.Interop.IServiceProvider oleSp = null;
            VsProject.GetItemContext(itemId, out oleSp);

            if (oleSp == null)
            {
                // Return E_FAIL to inform Visual Studio that the generator has failed (so that no file gets generated)
                pcbOutput = 0;
                return VSConstants.E_FAIL;
            }

            // convert our handle to a ProjectItem
            item = new ServiceProvider(oleSp).GetService(typeof(EnvDTE.ProjectItem))
                        as EnvDTE.ProjectItem;

            //compile the typeSql
            var compileResult = TypeSqlCompiler.Compile(bstrInputFileContents, Path.GetFileNameWithoutExtension(wszInputFilePath));

            //write the raw-sql file
            string sqlFilePath = Path.Combine(Path.GetDirectoryName(wszInputFilePath), Path.GetFileNameWithoutExtension(wszInputFilePath) + ".sql");
            using (var fileStream = File.CreateText(sqlFilePath))
            {
                fileStream.Write(compileResult.RawSql);
                fileStream.Close();
            }

            EnvDTE.ProjectItem sqlProjectItem = item.ProjectItems.AddFromFile(sqlFilePath);

            //now write the DAO-code to be written, via the normal interface the Generate method expects:
            // The contract between IVsSingleFileGenerator implementors and consumers is that 
            // any output returned from IVsSingleFileGenerator.Generate() is returned through  
            // memory allocated via CoTaskMemAlloc(). Therefore, we have to convert the 
            // byte[] array returned from GenerateCode() into an unmanaged blob.  

            var daoBytes = Encoding.UTF8.GetBytes(compileResult.Dao);
            int outputLength = daoBytes.Length;
            rgbOutputFileContents[0] = Marshal.AllocCoTaskMem(outputLength);
            Marshal.Copy(daoBytes, 0, rgbOutputFileContents[0], outputLength);

            pcbOutput = (uint)outputLength;
            return VSConstants.S_OK;



        }

        public void SetSite(object pUnkSite)
        {
            site = pUnkSite;
            codeDomProvider = null;
            serviceProvider = null;
        }

        public void GetSite(ref Guid riid, out IntPtr ppvSite)
        {
            if (site == null)
            {
                throw new COMException("object is not sited", VSConstants.E_FAIL);
            }

            IntPtr pUnknownPointer = Marshal.GetIUnknownForObject(site);
            IntPtr intPointer = IntPtr.Zero;
            Marshal.QueryInterface(pUnknownPointer, ref riid, out intPointer);

            if (intPointer == IntPtr.Zero)
            {
                throw new COMException("site does not support requested interface", VSConstants.E_NOINTERFACE);
            }

            ppvSite = intPointer;
        }

        /// <summary>
        /// Gets the default extension of the output file from the CodeDomProvider
        /// </summary>
        /// <returns></returns>
        private  string GetDefaultExtension()
        {
            CodeDomProvider codeDom = GetCodeProvider();
            Debug.Assert(codeDom != null, "CodeDomProvider is NULL.");
            string extension = codeDom.FileExtension;
            if (extension != null && extension.Length > 0)
            {
                extension = ".generated." + extension.TrimStart(".".ToCharArray());
            }
            return extension;
        }

        /// <summary>
        /// Returns a CodeDomProvider object for the language of the project containing
        /// the project item the generator was called on
        /// </summary>
        /// <returns>A CodeDomProvider object</returns>
        private  CodeDomProvider GetCodeProvider()
        {
            if (codeDomProvider == null)
            {
                //Query for IVSMDCodeDomProvider/SVSMDCodeDomProvider for this project type
                IVSMDCodeDomProvider provider = GetService(typeof(SVSMDCodeDomProvider)) as IVSMDCodeDomProvider;
                if (provider != null)
                {
                    codeDomProvider = provider.CodeDomProvider as CodeDomProvider;
                }
                else
                {
                    //In the case where no language specific CodeDom is available, fall back to C#
                    codeDomProvider = CodeDomProvider.CreateProvider("C#");
                }
            }
            return codeDomProvider;
        }

        /// <summary>
        /// Demand-creates a ServiceProvider
        /// </summary>
        private ServiceProvider SiteServiceProvider
        {
            get
            {
                if (serviceProvider == null)
                {
                    serviceProvider = new ServiceProvider(site as Microsoft.VisualStudio.OLE.Interop.IServiceProvider);
                    Debug.Assert(serviceProvider != null, "Unable to get ServiceProvider from site object.");
                }
                return serviceProvider;
            }
        }

        /// <summary>
        /// Method to get a service by its GUID
        /// </summary>
        /// <param name="serviceGuid">GUID of service to retrieve</param>
        /// <returns>An object that implements the requested service</returns>
        private object GetService(Guid serviceGuid)
        {
            return SiteServiceProvider.GetService(serviceGuid);
        }
        /// <summary>
        /// Method to get a service by its Type
        /// </summary>
        /// <param name="serviceType">Type of service to retrieve</param>
        /// <returns>An object that implements the requested service</returns>
        protected object GetService(Type serviceType)
        {
            return SiteServiceProvider.GetService(serviceType);
        }

        private IVsGeneratorProgress codeGeneratorProgress;
        private string codeFileNameSpace = String.Empty;
        private string codeFilePath = String.Empty;
        private object site = null;
        private CodeDomProvider codeDomProvider = null;
        private ServiceProvider serviceProvider = null;
        private EnvDTE.Project project;
    }
}
