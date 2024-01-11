-- Replaced with Table function [Utility].[GetPricingInfo]
IF OBJECT_ID('[Reports].[PricingInfo]') IS NOT NULL
    DROP FUNCTION [Reports].[PricingInfo];
GO