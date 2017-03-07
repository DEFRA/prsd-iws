IF OBJECT_ID('[Reports].[PotentialRefund]') IS NULL
    EXEC('CREATE FUNCTION [Reports].[PotentialRefund]() RETURNS MONEY AS BEGIN RETURN 0 END;')
GO	

-- Replaced with Table function [Reports].[PricingInfo]
DROP FUNCTION [Reports].[PotentialRefund];
GO