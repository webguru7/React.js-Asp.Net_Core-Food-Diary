import React, { useEffect } from 'react';
import './PageContent.scss';
import { useParams } from 'react-router-dom';
import { StateToPropsMapResult, DispatchToPropsMapResult } from './PageContentConnected';
import Loader from '../Loader';
import PageContentHeaderConnected from '../PageContentHeader';
import MealsControlPanelConnected from '../MealsControlPanel/MealsControlPanelConnected';

interface PageContentProps extends StateToPropsMapResult, DispatchToPropsMapResult {}

const PageContent: React.FC<PageContentProps> = ({ loading, loaded, getContent, errorMessage }: PageContentProps) => {
  const { id: pageId } = useParams();

  useEffect(() => {
    if (pageId && !isNaN(+pageId)) {
      getContent(+pageId);
    }
  }, [pageId, getContent]);

  if (loading) {
    return <Loader label="Loading page content"></Loader>;
  }

  if (loaded) {
    return (
      <div className="page-content">
        <PageContentHeaderConnected></PageContentHeaderConnected>
        <MealsControlPanelConnected></MealsControlPanelConnected>
      </div>
    );
  }

  return <div>{errorMessage}</div>;
};

export default PageContent;