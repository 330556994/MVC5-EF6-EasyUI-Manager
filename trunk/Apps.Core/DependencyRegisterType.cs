using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.Unity;
using Apps.IBLL;
using Apps.BLL;
using Apps.IDAL;
using Apps.DAL;
using Apps.MIS.IBLL;
using Apps.MIS.BLL;
using Apps.MIS.IDAL;
using Apps.MIS.DAL;
using Apps.DEF.IBLL;
using Apps.DEF.BLL;
using Apps.DEF.IDAL;
using Apps.DEF.DAL;
using Apps.Flow.IBLL;
using Apps.Flow.BLL;
using Apps.Flow.IDAL;
using Apps.Flow.DAL;
using Apps.Spl.IBLL;
using Apps.Spl.IDAL;
using Apps.Spl.BLL;
using Apps.Spl.DAL;
using Apps.WC.IBLL;
using Apps.WC.IDAL;
using Apps.WC.BLL;
using Apps.WC.DAL;

namespace Apps.Core
{
    public class DependencyRegisterType
    {
        //系统注入
        public static void Container_Sys(ref UnityContainer container)
        {
            container.RegisterType<ISysSampleBLL, SysSampleBLL>();//样例
            container.RegisterType<ISysSampleRepository, SysSampleRepository>();
            container.RegisterType<IHomeBLL, HomeBLL>();//首页
            container.RegisterType<IHomeRepository, HomeRepository>();
            container.RegisterType<IWebpartBLL, WebpartBLL>();//首页
            container.RegisterType<IWebpartRepository, WebpartRepository>();
            container.RegisterType<IAccountBLL, AccountBLL>();//用户
            container.RegisterType<IAccountRepository, AccountRepository>();
            container.RegisterType<ISysUserBLL, SysUserBLL>();//用户
            container.RegisterType<ISysUserRepository, SysUserRepository>();
            container.RegisterType<ISysRoleBLL, SysRoleBLL>();//角色
            container.RegisterType<ISysRoleRepository, SysRoleRepository>();
            container.RegisterType<ISysRightBLL, SysRightBLL>();//系统授权
            container.RegisterType<ISysRightRepository, SysRightRepository>();
            container.RegisterType<ISysModuleBLL, SysModuleBLL>();//模组
            container.RegisterType<ISysModuleRepository, SysModuleRepository>();
            container.RegisterType<ISysLogBLL, SysLogBLL>();//系统日志
            container.RegisterType<ISysLogRepository, SysLogRepository>();
            container.RegisterType<ISysExceptionBLL, SysExceptionBLL>();//异常日志
            container.RegisterType<ISysExceptionRepository, SysExceptionRepository>();
            container.RegisterType<ISysRightGetRoleRightBLL, SysRightGetRoleRightBLL>();//查看角色权限
            container.RegisterType<ISysRightGetRoleRightRepository, SysRightGetRoleRightRepository>();
            container.RegisterType<ISysRightGetUserRightBLL, SysRightGetUserRightBLL>();//查看用户权限
            container.RegisterType<ISysRightGetUserRightRepository, SysRightGetUserRightRepository>();
            container.RegisterType<ISysRightGetModuleRightBLL, SysRightGetModuleRightBLL>();//查看模块被赋予权限
            container.RegisterType<ISysRightGetModuleRightRepository, SysRightGetModuleRightRepository>();
            container.RegisterType<ISysModuleBLL, SysModuleBLL>();//模块设置
            container.RegisterType<ISysModuleRepository, SysModuleRepository>();
            container.RegisterType<ISysModuleOperateBLL, SysModuleOperateBLL>();//模块设置
            container.RegisterType<ISysModuleOperateRepository, SysModuleOperateRepository>();
            container.RegisterType<ISysAreasBLL, SysAreasBLL>();//区域设置
            container.RegisterType<ISysAreasRepository, SysAreasRepository>();
            container.RegisterType<ISysStructBLL, SysStructBLL>();//公司体系
            container.RegisterType<ISysStructRepository, SysStructRepository>();
            container.RegisterType<ISysPositionBLL, SysPositionBLL>();//公司职位
            container.RegisterType<ISysPositionRepository, SysPositionRepository>();
            container.RegisterType<ISysUserConfigBLL, SysUserConfigBLL>();//用户配置
            container.RegisterType<ISysUserConfigRepository, SysUserConfigRepository>();

            container.RegisterType<IJOB_TASKJOBSBLL, JOB_TASKJOBSBLL>();//用户配置
            container.RegisterType<IJOB_TASKJOBSRepository, JOB_TASKJOBSRepository>();
            container.RegisterType<IJOB_TASKJOBS_LOGBLL, JOB_TASKJOBS_LOGBLL>();//用户配置
            container.RegisterType<IJOB_TASKJOBS_LOGRepository, JOB_TASKJOBS_LOGRepository>();
        }


        //MIS注入
        public static void Container_Mis(ref UnityContainer container)
        {
            container.RegisterType<IMIS_WebIM_CommonTalkBLL, MIS_WebIM_CommonTalkBLL>();
            container.RegisterType<IMIS_WebIM_CommonTalkRepository, MIS_WebIM_CommonTalkRepository>();

            container.RegisterType<IMIS_WebIM_RecentContactBLL, MIS_WebIM_RecentContactBLL>();
            container.RegisterType<IMIS_WebIM_RecentContactRepository, MIS_WebIM_RecentContactRepository>();

            container.RegisterType<IMIS_WebIM_MessageBLL, MIS_WebIM_MessageBLL>();
            container.RegisterType<IMIS_WebIM_MessageRepository, MIS_WebIM_MessageRepository>();

            container.RegisterType<IMIS_ArticleBLL, MIS_ArticleBLL>();
            container.RegisterType<IMIS_ArticleRepository, MIS_ArticleRepository>();

            container.RegisterType<IMIS_Article_AlbumsBLL, MIS_Article_AlbumsBLL>();
            container.RegisterType<IMIS_Article_AlbumsRepository, MIS_Article_AlbumsRepository>();

            container.RegisterType<IMIS_Article_CategoryBLL, MIS_Article_CategoryBLL>();
            container.RegisterType<IMIS_Article_CategoryRepository, MIS_Article_CategoryRepository>();

            container.RegisterType<IMIS_Article_CommentBLL, MIS_Article_CommentBLL>();
            container.RegisterType<IMIS_Article_CommentRepository, MIS_Article_CommentRepository>();
        }

        //DEF注入
        public static void Container_Def(ref UnityContainer container)
        {
            container.RegisterType<IDEF_CaseTypeBLL, DEF_CaseTypeBLL>();
            container.RegisterType<IDEF_CaseTypeRepository, DEF_CaseTypeRepository>();

            container.RegisterType<IDEF_DefectBLL, DEF_DefectBLL>();
            container.RegisterType<IDEF_DefectRepository, DEF_DefectRepository>();

            container.RegisterType<IDEF_TestCaseBLL, DEF_TestCaseBLL>();
            container.RegisterType<IDEF_TestCaseRepository, DEF_TestCaseRepository>();

            container.RegisterType<IDEF_TestCaseRelationBLL, DEF_TestCaseRelationBLL>();
            container.RegisterType<IDEF_TestCaseRelationRepository, DEF_TestCaseRelationRepository>();

            container.RegisterType<IDEF_TestCaseStepsBLL, DEF_TestCaseStepsBLL>();
            container.RegisterType<IDEF_TestCaseStepsRepository, DEF_TestCaseStepsRepository>();

            container.RegisterType<IDEF_TestJobsBLL, DEF_TestJobsBLL>();
            container.RegisterType<IDEF_TestJobsRepository, DEF_TestJobsRepository>();

            container.RegisterType<IDEF_TestJobsDetailBLL, DEF_TestJobsDetailBLL>();
            container.RegisterType<IDEF_TestJobsDetailRepository, DEF_TestJobsDetailRepository>();

            container.RegisterType<IDEF_TestJobsDetailItemBLL, DEF_TestJobsDetailItemBLL>();
            container.RegisterType<IDEF_TestJobsDetailItemRepository, DEF_TestJobsDetailItemRepository>();

            container.RegisterType<IDEF_TestJobsDetailRelationBLL, DEF_TestJobsDetailRelationBLL>();
            container.RegisterType<IDEF_TestJobsDetailRelationRepository, DEF_TestJobsDetailRelationRepository>();

            container.RegisterType<IDEF_TestJobsDetailStepsBLL, DEF_TestJobsDetailStepsBLL>();
            container.RegisterType<IDEF_TestJobsDetailStepsRepository, DEF_TestJobsDetailStepsRepository>();
        }

        //Flow注入
        public static void Container_Flow(ref UnityContainer container)
        {
            container.RegisterType<IFlow_TypeBLL, Flow_TypeBLL>();
            container.RegisterType<IFlow_TypeRepository, Flow_TypeRepository>();

            container.RegisterType<IFlow_FormAttrBLL, Flow_FormAttrBLL>();
            container.RegisterType<IFlow_FormAttrRepository, Flow_FormAttrRepository>();

            container.RegisterType<IFlow_FormBLL, Flow_FormBLL>();
            container.RegisterType<IFlow_FormRepository, Flow_FormRepository>();

            container.RegisterType<IFlow_StepBLL, Flow_StepBLL>();
            container.RegisterType<IFlow_StepRepository, Flow_StepRepository>();

            container.RegisterType<IFlow_StepRuleBLL, Flow_StepRuleBLL>();
            container.RegisterType<IFlow_StepRuleRepository, Flow_StepRuleRepository>();

            container.RegisterType<IFlow_FormContentBLL, Flow_FormContentBLL>();
            container.RegisterType<IFlow_FormContentRepository, Flow_FormContentRepository>();

            container.RegisterType<IFlow_FormContentStepCheckBLL, Flow_FormContentStepCheckBLL>();
            container.RegisterType<IFlow_FormContentStepCheckRepository, Flow_FormContentStepCheckRepository>();

            container.RegisterType<IFlow_FormContentStepCheckStateBLL, Flow_FormContentStepCheckStateBLL>();
            container.RegisterType<IFlow_FormContentStepCheckStateRepository, Flow_FormContentStepCheckStateRepository>();
        }
        /// <summary>
        /// Spl模块注入
        /// </summary>
        /// <param name="container"></param>
        public static void Container_Sql(ref UnityContainer container)
        {
            container.RegisterType<ISpl_ProductCategoryBLL, Spl_ProductCategoryBLL>();
            container.RegisterType<ISpl_ProductCategoryRepository, Spl_ProductCategoryRepository>();

            container.RegisterType<ISpl_ProductBLL, Spl_ProductBLL>();
            container.RegisterType<ISpl_ProductRepository, Spl_ProductRepository>();

            container.RegisterType<ISpl_PersonBLL, Spl_PersonBLL>();
            container.RegisterType<ISpl_PersonRepository, Spl_PersonRepository>();
        }


        /// <summary>
        /// WC模块注入
        /// </summary>
        /// <param name="container"></param>
        public static void Container_WC(ref UnityContainer container)
        {

            container.RegisterType<IWC_MessageResponseBLL, WC_MessageResponseBLL>();
            container.RegisterType<IWC_MessageResponseRepository, WC_MessageResponseRepository>();

            container.RegisterType<IWC_OfficalAccountsBLL, WC_OfficalAccountsBLL>();
            container.RegisterType<IWC_OfficalAccountsRepository, WC_OfficalAccountsRepository>();

            container.RegisterType<IWC_ResponseLogBLL, WC_ResponseLogBLL>();
            container.RegisterType<IWC_ResponseLogRepository, WC_ResponseLogRepository>();

            container.RegisterType<IWC_UserBLL, WC_UserBLL>();
            container.RegisterType<IWC_UserRepository, WC_UserRepository>();

            container.RegisterType<IWC_GroupBLL, WC_GroupBLL>();
            container.RegisterType<IWC_GroupRepository, WC_GroupRepository>();
        }
    }
}