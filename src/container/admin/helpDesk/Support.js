import React, { useEffect, useState, useRef} from 'react';
import { useForm } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import { useDispatch, useSelector } from 'react-redux';
import Grid from "@material-ui/core/Grid";
import { Typography, Button, IconButton } from '@mui/material';
import { styled } from '@mui/material/styles';
import { alertActions, supportActions } from '_store';
import { SupportLabels } from '_utils/labels';
import { supportInformationSchema } from '_utils/validationSchema';
import { supportSupportedFormat } from '_utils/constant';
import { convertToBase64, base64ToFile, fileExtension, fileSizeReadable, fileTypeAcceptable } from '_utils';
import { Delete } from '../../../images';
import { materialsymbolsupload, materialsymbolsdownload } from 'images';
import SupportDetails from './SupportDetails';

const formatPhoneNumber = (number) => {
      const cleaned = ('' + number).replace(/\D/g, '');
      const match = cleaned.match(/^(\d{3})(\d{3})(\d{4})$/);
      if (match) {
            return `${match[1]}-${match[2]}-${match[3]}`;
      }
      return number;
};

const VisuallyHiddenInput = styled('input')({
      clip: 'rect(0 0 0 0)',
      clipPath: 'inset(50%)',
      height: 1,
      overflow: 'hidden',
      position: 'absolute',
      bottom: 0,
      left: 0,
      whiteSpace: 'nowrap',
      width: 1,
});

const Support = () => {
      const header = "Support";
      const maxFileSize = 5000000;
      const minFileSize = 0;
      const multiple = true;
      const dispatch = useDispatch();
      const support = useSelector(x => x.supports?.supportDetails);
      const [files, setFiles] = useState([]);
      const authUser = useSelector(x => x.auth?.value);
      const authUserId = useSelector(x => x.auth?.userId);
      const user = authUser?.Data;
      const [Supportdata, setSupportData] = useState(null);
      const [initialData, setInitialData] = useState({});
      const [isModified, setIsModified] = useState(false);
      const adminList = user?.UserAccess?.filter(access => access.Role.toLowerCase() === "admin");
      const initialDataRef = useRef({});
      const portalsList = adminList ? adminList?.map(admin => ({
            PortalId: admin.PortalId,
            PortalName: admin.PortalName,
            PortalKey: admin.PortalKey,
      })) : [];

      const defaultPortalId = (portalsList && portalsList[0].PortalId) || null;

      const portalData = portalsList ? portalsList?.map(x => ({
            label: x.PortalName,
            value: x.PortalId
      })) : [];

      portalData.push({ label: "Global", value: 99 });
      portalData.push({ label: "Admin", value: 98 });

      const [selectedPortal, setSelectedPortal] = useState(defaultPortalId);

      const { register, handleSubmit, control, trigger, reset, setValue, formState: { errors, isValid }, watch } = useForm({
            resolver: yupResolver(supportInformationSchema),
            mode:'onBlur',
            defaultValues: {
                  EmailAddress: ' ',
                  PhoneNumber: ' ',
                  Fax: ' ',
                  PortalID: defaultPortalId,
            }
      });

      const isDeepEqual = (obj1, obj2) => {
            if (obj1 === obj2) return true;

            if (typeof obj1 !== "object" || typeof obj2 !== "object" || obj1 === null || obj2 === null) {
                  return obj1 === obj2;
            }

            const keys1 = Object.keys(obj1);
            const keys2 = Object.keys(obj2);

            if (keys1.length !== keys2.length) return false;

            for (let key of keys1) {
                  if (!isDeepEqual(obj1[key], obj2[key])) {
                        return false;
                  }
            }
            return true;
      };
      useEffect(() => {
            const subscription = watch((currentData) => {
                  const isChanged = !isDeepEqual(initialDataRef.current, currentData);
                  setIsModified(isChanged);
            });

            return () => subscription.unsubscribe();
      }, [watch]);

      useEffect(() => {
            const fetchData = async () => {
                  dispatch(alertActions.clear());
                  try {
                        const result = await dispatch(supportActions.getSupportDetails(selectedPortal)).unwrap();
                        const supportData = result?.Data;
                        setSupportData(result.Data);
                        sessionStorage.setItem('portalID', selectedPortal);
                        const data = { ...supportData, PhoneNumber: formatPhoneNumber(supportData.PhoneNumber), Fax: formatPhoneNumber(supportData.Fax) };
                        if (supportData?.FileData) {
                              setFiles(supportData?.FileData);
                        }
                        setInitialData(data);
                        initialDataRef.current = data;
                        reset(data);
                      
                        if (result?.error) {
                              dispatch(alertActions.error({
                                    message: result.error?.message,
                                    header: header
                              }));
                              reset();
                        }
                  } catch (error) {
                        dispatch(alertActions.error({
                              message: error?.message || error,
                              header: header
                        }));
                        reset();
                  }
            };
            fetchData();
      }, [selectedPortal, dispatch, reset]);

      // useEffect(() => {
      //       setValue('PortalID', selectedPortal);
      // }, [selectedPortal, setValue]);

      const onSubmit = async (data) => {
            dispatch(alertActions.clear());
            try {
                  data = { ...data, PortalID: selectedPortal, CreatedBy: authUserId.toString(), SupportID: support?.SupportID, FileData: files };

                  const result = await dispatch(supportActions.updateSupportDetails({ data }));

                  if (result?.error) {
                        dispatch(alertActions.error({ message: result?.payload || result?.error.message, header: header }));
                        return;
                  }
                  hanldeRefresh();
                  setSupportData(result.Data);
                  dispatch(alertActions.success({ message: SupportLabels.message1, header: SupportLabels.header, showAfterRedirect: true }));
            } catch (error) {
                  dispatch(alertActions.error({ message: error?.message || error, header: header }));
            }
      };

      const hanldeRefresh = async () => {
            const result = await dispatch(supportActions.getSupportDetails(selectedPortal)).unwrap();
            const supportData = result?.Data;
            setSupportData(result.Data);
            const data = { ...supportData, PhoneNumber: formatPhoneNumber(supportData.PhoneNumber), Fax: formatPhoneNumber(supportData.Fax) };
            if (supportData?.FileData) {
                  setFiles(supportData?.FileData);
            }
            initialDataRef.current = data;
            setInitialData(data);
            reset(data);
          
      }

      const handlePortalChange = (event, newValue) => {
            // resetValue();
            setSelectedPortal(newValue);

      };

      const handleBlur = async (e) => {
            const fieldName = e.target.name;
            // console.log(`Triggering validation for: ${fieldName}`);
            await trigger(fieldName);
            //  console.log(`Validation result for ${fieldName}:`, result);
      };

      const handleDownload = (base64String, fileName) => {
            base64ToFile(base64String, fileName);
      };

      const handleUpload = async (event) => {
            let filesAdded = event.dataTransfer ? event.dataTransfer.files : event.target.files;
            if (!filesAdded || filesAdded.length === 0) return;

            if (multiple === false && filesAdded.length > 1) {
                  filesAdded = [filesAdded[0]];
            }

            const fileResults = [];
            const errorMessages = [];

            for (let i = 0; i < filesAdded.length; i += 1) {
                  const file = filesAdded[i];
                  file.extension = fileExtension(file);
                  file.sizeReadable = fileSizeReadable(file.size);

                  if (file.size > maxFileSize) {
                        dispatch(alertActions.error({ message: `File size cannot exceed 5 MB`, header: header }));
                        continue;
                  }

                  if (file.size < minFileSize) {
                        dispatch(alertActions.error({ message: `File is too small`, header: header }));
                        continue;
                  }

                  if (!fileTypeAcceptable(supportSupportedFormat, file)) {
                        dispatch(alertActions.error({ message: `Invalid file format`, header: header }));
                        continue;
                  }

                  const base64 = await convertToBase64(file);

                  const fileData = {
                        ID: null,
                        AdditionalID: 0,
                        DocumentTypeID: 0,
                        FileName: file.name,
                        Format: file.extension,
                        Size: file.size,
                        PortalKey: "TM",
                        File: base64,
                        Url: null
                  };

                  fileResults.push(fileData);
            }

            if (errorMessages.length > 0) {
                  dispatch(alertActions.error(errorMessages.join('\n')));
            }

            if (fileResults.length > 0) {
                  setFiles(prevFiles => [...prevFiles, ...fileResults]);
            }

            // Reset input value so selecting same file again triggers onChange
            event.target.value = null;
      };


      const handleError = (error, file) => {
            dispatch(alertActions.error(error.message));
      };

      const handleCancelClick = async () => {
            await setFiles(support?.FileData);
            reset(support);
            hanldeRefresh();
      }

      const handleFileRemove = (fileName) => {
            setFiles(prevFiles => prevFiles.filter(prevFile => prevFile.FileName !== fileName))
      }

      return (
            <Typography component="div" className="suportcontent">
                  <Typography component="div">
                        <Typography component="h1" variant="h5" className='userprofilelistcontent '>Support</Typography>
                  </Typography>
                  {Supportdata && (
                        <form onSubmit={handleSubmit(onSubmit)}>
                              <Typography className="suportcontentcontainer" component="div">
                                    <Grid container spacing={3}>
                                          <SupportDetails
                                                register={register}
                                                control={control}
                                                errors={errors}
                                                handleBlur={handleBlur}
                                                handlePortalChange={handlePortalChange}
                                                portalData={portalData}
                                                selectedPortal={selectedPortal}
                                                trigger={trigger}
                                          />
                                          <Grid item xs={12} sm={6} md={4} className='supplierDetailes'>
                                                <Typography component="div" className="SupportedFormats Personal-Informationsheading">
                                                      <Typography component="h2">Uploaded Documents</Typography>
                                                      <Typography component="div" >
                                                            <Typography component="h3">Reference Documents</Typography>
                                                            {files && files.map((file) =>
                                                                  <Typography component="div" className="marginbottom">
                                                                        <Typography component="span" className="DocumentDescription">{file?.FileName}</Typography>
                                                                        <Typography component="div" className="DocumentTypeID">
                                                                              <IconButton onClick={() => handleDownload(file?.File, file?.FileName)}>
                                                                                    {/* <DownloadIcon variant="contained" color="secondary" /> */}
                                                                                    <img src={materialsymbolsdownload} alt='download'></img>
                                                                              </IconButton>
                                                                              <IconButton onClick={() => handleFileRemove(file?.FileName)}>
                                                                                    {/* <Delete variant="contained" color="secondary" /> */}
                                                                                    <img src={Delete} alt="Delete" ></img>
                                                                              </IconButton>
                                                                        </Typography>
                                                                  </Typography>
                                                            )}
                                                      </Typography>
                                                </Typography>
                                          </Grid>
                                          <Grid item xs={12} sm={6} md={4} className='supplierDetailes'>
                                                <Typography component="div" className="SupportedFormats">
                                                      <Typography component="div" className=" Personal-Informationsheading" >
                                                            <Typography component="h2" variant="h5" >Documents upload </Typography>
                                                            <Typography component="div" className='UploadContainer'>
                                                                  <Grid container spacing={3}>
                                                                        <Grid item xs={12} sm={6} md={12}>
                                                                              <Button
                                                                                    component="label"
                                                                                    role={undefined}
                                                                                    tabIndex={-1}
                                                                                    className="Uploadfiles"
                                                                                    startIcon={<img src={materialsymbolsupload} alt="Upload" />}>
                                                                                    <span> Upload your files here</span>
                                                                                    <VisuallyHiddenInput
                                                                                          type="file"
                                                                                          onChange={handleUpload}
                                                                                          multiple
                                                                                    />
                                                                              </Button>
                                                                        </Grid>
                                                                        <Grid item xs={12} sm={6} md={12}>
                                                                              <Typography component="div" className="SupportedFormats">
                                                                                    <Typography component="h3">Supported Formats</Typography>
                                                                                    <Typography component="div" className="fileformatlist">
                                                                                          {supportSupportedFormat && supportSupportedFormat.map(format => <span key={format}>{format}</span>)}
                                                                                    </Typography>
                                                                              </Typography>
                                                                        </Grid>
                                                                  </Grid>
                                                            </Typography>
                                                      </Typography>
                                                </Typography>
                                          </Grid>

                                    </Grid>
                              </Typography>
                              <Grid item xs={12} sm={12} md={12} className="Personal-Information">
                                    <Button variant="contained" color="red" className="cancelbutton" onClick={handleCancelClick}>
                                          Cancel
                                    </Button>
                                    <Button type="submit" variant="contained" className='submitbutton' color="primary" disabled={!isModified || !isValid}>
                                          Save
                                    </Button>
                              </Grid>


                        </form>
                  )}
                  {/* {support?.error && <UnderConstruction />} */}
            </Typography>
      );
}

export default Support;